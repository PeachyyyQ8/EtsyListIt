using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EtsyListIt.Utility.DomainObjects;
using EtsyListIt.Utility.Interfaces;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using StructureMap;
using EtsyListIt.Utility.Extensions;
using IllustratorWrapper;

namespace EtsyListIt
{
    class Program
    {
        private static Container _container;
        private static ISettingsUtility _settingsUtility;
        private static IListingWrapper _listingWrapper;
        private static IIllustratorActionWrapper _illustratorActionWrapper;
        private static ISystemUtility _systemUtility;

        static void Main(string[] args)
        {
            try
            {
                #region IOC
                _container = ConfigureStructureMap();
                _settingsUtility = _container.GetInstance<ISettingsUtility>();
                var commandLineUtility = _container.GetInstance<ICommandLineUtility>();
                _listingWrapper = _container.GetInstance<IListingWrapper>();
                _illustratorActionWrapper = _container.GetInstance<IIllustratorActionWrapper>();
                _systemUtility = _container.GetInstance<ISystemUtility>();

                #endregion

                var listItArgs = commandLineUtility.ParseCommandLineArguments(args);
                
                if (listItArgs.WorkingDirectory.IsNullOrEmpty())
                {
                    throw new EtsyListItException("Working directory can not be empty.");
                }
                var baseFiles = Directory.GetFiles(listItArgs.WorkingDirectory).Where(x => x.Contains(".svg")).ToList();
                if (!baseFiles.Any())
                {
                    throw new EtsyListItException("There are no files to list!");
                }
                foreach (var baseFile in baseFiles)
                {
                    #region Illustrator File Creation & Export
                    if (listItArgs.OutputDirectory.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("Output directory can not be empty.");
                    }
                    var outputDirectory = Path.Combine(listItArgs.OutputDirectory, Path.GetFileNameWithoutExtension(baseFile));

                    ExportFiles(baseFile, outputDirectory, listItArgs.WatermarkFile);
                    #endregion
                    Directory.CreateDirectory(outputDirectory);
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Directory.CreateDirectory(Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(baseFile)));
                    var zipFile = _systemUtility.CreateZipFileFromDirectory(baseFile, outputDirectory);
                    var ignoredContent = new List<string> { ".svg", ".zip"};
                    _systemUtility.DeleteFilesInDirectory(outputDirectory, ignoredContent);
                    var watermark = _illustratorActionWrapper.SaveFileWithWatermark(listItArgs.WatermarkFile, baseFile, outputDirectory);

                    #region Begin Etsy Export

                    var authToken = GetAuthToken(listItArgs);

                    if (!authToken.IsValidEtsyToken())
                    {
                        throw new EtsyListItException(
                            "Invalid AuthToken.  Please use the correct Verifier key obtained from Etsy to generate your permanent token credentials.");
                    }

                    var listing = PopulateGraphicListing(watermark, zipFile, listItArgs);
                    listing = _listingWrapper.CreateDigitalListingWithImage(listing, authToken);

                    #endregion

                    Console.Write($"Listing created. New listing ID: {listing.ID} Press any key to end.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Write("Type any key to quit.");
                Console.ReadLine();
            }
        }


        private static void ExportFiles(string baseFile, string outputDirectory, string watermarkFile)
        {
            _illustratorActionWrapper.ExportFileAsJPEG(baseFile, outputDirectory);
            _illustratorActionWrapper.ExportFileAsPNG(baseFile, outputDirectory);
            _illustratorActionWrapper.ExportFileAsDXF(baseFile, outputDirectory);
            _illustratorActionWrapper.SaveFileAsEPS(baseFile, outputDirectory);
            _illustratorActionWrapper.SaveFileAsPDF(baseFile, outputDirectory);
            _illustratorActionWrapper.SaveFileWithWatermark(watermarkFile, baseFile, outputDirectory);
        }


        private static Listing PopulateGraphicListing(string watermarkPath, string digitalFilePath, EtsyListItArgs listItArgs)
        {
            if (listItArgs.ListingCustomTitle.IsNullOrEmpty())
            {
                throw new EtsyListItException(
                    "User must provide custom title for listing.  Use command line arg -ct to specify.");
            }
            var listing = new Listing
            {
                Title = $"{listItArgs.ListingCustomTitle} {listItArgs.ListingDefaultTitle}",
                Description = $"{listItArgs.ListingCustomTitle}\r\n{listItArgs.ListingDefaultDescription}",
                Quantity = listItArgs.ListingQuantity,
                Price = decimal.TryParse(listItArgs.ListingPrice, out decimal price)
                    ? price
                    : throw new InvalidDataException("Price must be a decimal value."),
                IsSupply = true,
                CategoryID = "69150433",
                WhenMade = "2010_2017",
                WhoMade = "i_did",
                IsCustomizable = true,
                IsDigital = true,
                ShippingTemplateID = "30116314577",
                Images = new[]
                {
                    new ListingImage
                    {
                        ImagePath = File.Exists(watermarkPath) ? watermarkPath : throw new InvalidDataException("Watermark path is invalid."),
                        Overwrite = true,
                        IsWatermarked = true,
                        Rank = 1
                    }
                },
                DigitalFiles = new[]
                {
                    new DigitalFile
                    {
                        Path = File.Exists(digitalFilePath) ? digitalFilePath: throw new InvalidDataException("Digital file path is invalid."),
                        Name = Path.GetFileName(digitalFilePath),
                        Rank = 1
                    }
                },
                Tags = listItArgs.ListingTags.Split(',')
            };

            return listing;
        }

        private static Container ConfigureStructureMap()
        {
            return new Container(new DependencyRegistry());
        }

        private static PermanentToken GetAuthToken(EtsyListItArgs args)
        {
            var permanentToken = new PermanentToken();
            
            // If API Key or Shared Secret not found either place, throw error.
            if (args.APIKey.IsNullOrEmpty() || args.SharedSecret.IsNullOrEmpty())
            {
                throw new EtsyListItException(
                    "Must specify API Key and Shared Secret to authenticate.  Please input command line arg -a [value] for API Key and -ss [value] for SharedSecret to continue.");
            }

            permanentToken.APIKey = args.APIKey;
            permanentToken.SharedSecret = args.SharedSecret;
            permanentToken.TokenID = _settingsUtility.GetEncryptedAppSetting("PermanentAuthToken");
            permanentToken.TokenSecret = _settingsUtility.GetEncryptedAppSetting("PermanentSecret");
            // If permanent auth token is not found, get it.
            if (permanentToken.TokenID.IsNullOrEmpty() || permanentToken.TokenSecret.IsNullOrEmpty())
            {
                var permissions = new[] {"listings_r", "listings_w"};
                var validator = string.Empty;
                IEtsyAuthenticationWrapper authenticationWrapper =
                    _container.GetInstance<IEtsyAuthenticationWrapper>();
                var tempCredentials =
                    authenticationWrapper.GetTemporaryCredentials(args.APIKey, args.SharedSecret, permissions);
                Process.Start(tempCredentials.LoginURL);
                while (validator.IsNullOrEmpty())
                {
                    Console.Write("Enter your Etsy validator from the web browser to continue: ");
                    validator = Console.ReadLine();
                }

                permanentToken =
                    authenticationWrapper.GetPermanentTokenCredentials(args.APIKey, args.SharedSecret, tempCredentials, validator);
            }
            
            _settingsUtility.SetAppSettingWithEncryption("PermanentAuthToken", permanentToken.TokenID);
            _settingsUtility.SetAppSettingWithEncryption("PermanentSecret", permanentToken.TokenSecret);
            
            return permanentToken;
        }
    }
}