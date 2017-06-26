using System;
using System.Diagnostics;
using System.IO;
using EtsyListIt.Utility.DomainObjects;
using EtsyListIt.Utility.Interfaces;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using StructureMap;
using EtsyListIt.Utility.Extensions;

namespace EtsyListIt
{
    class Program
    {
        private static Container _container;
        private static ISettingsUtility _settingsUtility;
        private static EtsyListItArgs _listItArgs;
        private static IListingWrapper _listingWrapper;

        static void Main(string[] args)
        {
            try
            {
                #region IOC

                _container = ConfigureStructureMap();
                _settingsUtility = _container.GetInstance<ISettingsUtility>();
                var commandLineUtility = _container.GetInstance<ICommandLineUtility>();
                _listingWrapper = _container.GetInstance<IListingWrapper>();

                #endregion

                _listItArgs = commandLineUtility.ParseCommandLineArguments(args);

                var authToken = GetAuthToken(_listItArgs);


                if (!authToken.IsValidEtsyToken())
                {
                    throw new EtsyListItException(
                        "Invalid AuthToken.  Please use the correct Verifier key obtained from Etsy to generate your permanent token credentials.");
                }

                var listing = PopulateGraphicListing(_listItArgs.WorkingDirectory + @"\thWatermark.jpg", _listItArgs.WorkingDirectory + @"\th.zip");
                listing = _listingWrapper.CreateDigitalListingWithImage(listing)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Write("Type any key to quit.");
                Console.ReadLine();
            }
        }

        private static DigitalListingWithImages PopulateGraphicListing(string watermarkPath, string digitalFilePath)
        {
            if (_listItArgs.ListingCustomTitle.IsNullOrEmpty())
            {
                throw new EtsyListItException(
                    "User must provide custom title for listing.  Use command line arg -ct to specify.");
            }
            var listing = new DigitalListingWithImages
            {
                Title = $"{_listItArgs.ListingCustomTitle} {_listItArgs.ListingDefaultTitle}",
                Description = $"{_listItArgs.ListingCustomTitle}\r\n{_listItArgs.ListingDefaultDescription}",
                Quantity = int.TryParse(_listItArgs.ListingQuantity, out int quantity)
                    ? quantity
                    : throw new InvalidDataException("Quantity must be a whole number."),
                Price = decimal.TryParse(_listItArgs.ListingPrice, out decimal price)
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
                Tags = _listItArgs.ListingTags.Split(',')
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