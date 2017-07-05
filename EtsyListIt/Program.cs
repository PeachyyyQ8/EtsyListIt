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

                #region File System Exception Handling
                if (listItArgs.WorkingDirectory.IsNullOrEmpty())
                {
                    throw new EtsyListItException("Working directory can not be empty.");
                }

                if (listItArgs.OutputDirectory.IsNullOrEmpty())
                {
                    throw new EtsyListItException("Output directory can not be empty.");
                }

                var baseFiles = Directory.GetFiles(listItArgs.WorkingDirectory).Where(x => x.Contains(".ai")).ToList();

                if (!baseFiles.Any())
                {
                    throw new EtsyListItException("There are no files to list!");
                }
                #endregion

                foreach (var baseFile in baseFiles)
                {
                    var tempDirectoryPath = Path.Combine(listItArgs.WorkingDirectory, Path.GetFileNameWithoutExtension(baseFile));
                    Directory.CreateDirectory(tempDirectoryPath);
                    
                    try
                    {
                        var artboardCount = _illustratorActionWrapper.CountArtboards(baseFile);
                        
                        var zipFiles = new List<string>();
                        var watermarks = new List<string>();

                        if (artboardCount > 1)
                        {
                            HandleMultipleArtboards(tempDirectoryPath, baseFile, artboardCount, listItArgs.WatermarkFile, out zipFiles, out watermarks);
                        }
                        else
                        {
                            zipFiles.Add(_systemUtility.CreateZipFileFromDirectory(baseFile, tempDirectoryPath));
                        }


                        bool.TryParse(listItArgs.AddToEtsy, out bool addToEtsy);

                        #region Etsy Export
                        if (addToEtsy)
                        {
                            Console.Write("Enter custom title: ");
                            var customTitle = Console.ReadLine();
                            Console.Write("Enter price: ");
                            var price = Console.ReadLine();
                            Console.Write("Enter tags: ");
                            var tags = Console.ReadLine();
                            
                            var authToken = GetAuthToken(listItArgs);

                            if (!authToken.IsValidEtsyToken())
                            {
                                throw new EtsyListItException(
                                    "Invalid AuthToken.  Please use the correct Verifier key obtained from Etsy to generate your permanent token credentials.");
                            }

                            var listing = PopulateGraphicListing(watermarks, zipFiles, listItArgs, customTitle, price, tags);
                            listing = _listingWrapper.CreateDigitalListingWithImages(listing, authToken);
                            
                            Console.Write($"Listing created. New listing ID: {listing.ID}");
                            Directory.Delete(tempDirectoryPath);
                            File.Move(baseFile, Path.Combine(listItArgs.OutputDirectory, Path.GetFileName(baseFile)));
                        }
                        else
                        {
                            Console.WriteLine("Add listing to Etsy feature turned OFF.  Use command line arg -add true to turn on.");
                        }
                        #endregion

                        Console.Write("Press any key to end.");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Directory.Delete(tempDirectoryPath, true);
                        throw;
                    }
                }
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
                Console.Write("Type any key to quit.");
                Console.ReadLine();

            }
        }

        private static void HandleMultipleArtboards(string tempDirectoryPath, string baseFile, int artboardCount, string watermarkPath,
            out List<string> zipFiles, out List<string> watermarks)
        {
            _illustratorActionWrapper.ExportMultipleArtboards(baseFile, tempDirectoryPath);
            zipFiles = new List<string>();
            watermarks = new List<string>();

            if (GetDirectorySize(tempDirectoryPath) <= 20000000) // ZIP FILES CAN'T BE MORE THAN 20MB
            {
                zipFiles.Add(_systemUtility.CreateZipFileFromDirectory(baseFile, tempDirectoryPath));
                return;
            }

            GroupFilesByType(tempDirectoryPath);

            var subDirectories = Directory.GetDirectories(tempDirectoryPath);

            if (subDirectories.Length < 1)
            {
                throw new EtsyListItException("An error has occurred in creating subdirectories for images.");
            }
            if (subDirectories.Length < 5)
            {
                CombineSmallestDirectories(tempDirectoryPath);
            }
            foreach (var subDirectory in Directory.GetDirectories(tempDirectoryPath))
            {
                var zip =
                    _systemUtility.CreateZipFileFromDirectory(subDirectory.Substring(subDirectory.LastIndexOf(@"\")),
                        subDirectory);
                zipFiles.Add(zip);
            }
            foreach (var zip in zipFiles)
            {
                var fileInfo = new FileInfo(zip);
                if (fileInfo.Length > 20000000)
                {
                    throw new EtsyListItException("Unable to list files.  File size too large.");
                }
            }

            if (artboardCount > 5)
            {
                Console.WriteLine("Too many artboards to watermark correctly.  Please check your watermarks.");
            }

            artboardCount = artboardCount > 5 ? 5 : artboardCount;

            for (int i = 0; i > artboardCount; i++)
            {
                var watermark = _illustratorActionWrapper.CreateWatermarkForArtboard(baseFile, tempDirectoryPath, watermarkPath, i);
                watermarks.Add(watermark);
            }

        }

        private static void CombineSmallestDirectories(string tempDirectoryPath)
        {
            var orderedBySize = (from d in Directory.GetDirectories(tempDirectoryPath)
                                 select new {name = d, size = GetDirectorySize(d)}).ToList();
            orderedBySize = orderedBySize.OrderBy(x => x.size).ToList();

            while (orderedBySize.Count() < 5)
            {
                var first = orderedBySize[0].name.Substring(orderedBySize[0].name.LastIndexOf(@"\"));
                var second = orderedBySize[1].name.Substring(orderedBySize[1].name.LastIndexOf(@"\"));
                var newDirectory = Path.Combine(tempDirectoryPath, first + second);
                Directory.CreateDirectory(newDirectory);
                foreach (var file in Directory.GetFiles(orderedBySize[0].name))
                {
                    File.Move(file, Path.Combine(newDirectory, Path.GetFileName(file)));
                }
                foreach (var file in Directory.GetFiles(orderedBySize[1].name))
                {
                    File.Move(file, Path.Combine(newDirectory, Path.GetFileName(file)));
                }

                Directory.Delete(orderedBySize[0].name, true);
                Directory.Delete(orderedBySize[1].name, true);
            }
        }

        private static void GroupFilesByType(string tempDirectoryPath)
        {
            var files = Directory.GetFiles(tempDirectoryPath);
            var extensions = new List<string>();
            foreach (var file in files)
            {
                // Find all distinct extensions
                var ex = file.Substring(file.IndexOf("."));
                if (!extensions.Contains(ex))
                {
                    extensions.Add(ex);
                }
            }

            foreach (var ex in extensions)
            {
                var subDirectory = Path.Combine(tempDirectoryPath, ex.Substring(1));

                //Create subdirectories for extensions
                Directory.CreateDirectory(subDirectory);
                
                //Get all files with the extension
                var subfiles = Directory.GetFiles(tempDirectoryPath).Where(x => x.Contains(ex));

                //now move files to subdirectory
                foreach (var file in subfiles)
                {
                    File.Move(file, subDirectory);
                }
            }


        }

        public static long GetDirectorySize(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = directoryInfo.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            return size;
        }

        private static Listing PopulateGraphicListing(List<string> watermarkFiles, List<string> digitalFilePaths,
            EtsyListItArgs listItArgs, string customTitle, string price, string tags)
        {
            if (listItArgs.ListingCustomTitle.IsNullOrEmpty())
            {
                throw new EtsyListItException(
                    "User must provide custom title for listing.  Use command line arg -ct to specify.");
            }
            var listing = new Listing
            {
                Title = $"{listItArgs.ListingCustomTitle} {listItArgs.ListingDefaultTitle}",
                Description =
                    $"{listItArgs.ListingCustomTitle} {listItArgs.ListingDefaultTitle}\r\n{listItArgs.ListingDefaultDescription}",
                Quantity = listItArgs.ListingQuantity,
                Price = decimal.TryParse(listItArgs.ListingPrice, out decimal priceValue)
                    ? priceValue
                    : throw new InvalidDataException("Price must be a decimal value."),
                IsSupply = true,
                CategoryID = "69150433",
                WhenMade = "2010_2017",
                WhoMade = "i_did",
                IsCustomizable = true,
                IsDigital = true,
                ShippingTemplateID = "30116314577",
                Tags = listItArgs.ListingTags.Split(',')
            };

            listing.Images = new ListingImage[5];

            var count = 0;
            foreach (var watermarkPath in watermarkFiles)
            {
                listing.Images[count] = new ListingImage
                {
                    ImagePath = File.Exists(watermarkPath)
                        ? watermarkPath
                        : throw new InvalidDataException("Watermark path is invalid."),
                    Overwrite = true,
                    IsWatermarked = true,
                    Rank = count
                };

            }

            listing.DigitalFiles = new DigitalFile[5];
            count = 0;
            foreach (var digitalFilePath in digitalFilePaths)
            {
                listing.DigitalFiles[count] = new DigitalFile
                {
                    Path = File.Exists(digitalFilePath)
                        ? digitalFilePath
                        : throw new InvalidDataException("Digital file path is invalid."),
                    Name = Path.GetFileName(digitalFilePath),
                    Rank = count
                };

                count++;
            }


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