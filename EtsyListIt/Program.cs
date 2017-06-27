using System;
using System.ComponentModel.Design;
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

        static void Main(string[] args)
        {
            try
            {
                #region IOC

                _container = ConfigureStructureMap();
                _settingsUtility = _container.GetInstance<ISettingsUtility>();
                var commandLineUtility = _container.GetInstance<ICommandLineUtility>();

                #endregion

                _listItArgs = commandLineUtility.ParseCommandLineArguments(args);

                var authToken = GetAuthToken(_listItArgs);


                if (!authToken.IsValidEtsyToken())
                {
                    throw new EtsyListItException(
                        "Invalid AuthToken.  Please use the correct Verifier key obtained from Etsy to generate your permanent token credentials.");
                }

                var listing = PopulateListing();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Write("Type any key to quit.");
                Console.ReadLine();
            }
        }

        private static Listing PopulateListing()
        {
            if (_listItArgs.ListingCustomTitle.IsNullOrEmpty())
            {
                throw new EtsyListItException("User must provide custom title for listing.  Use command line arg -ct to specify.");
            }
            Listing listing = new Listing
            {
                Title = $"{_listItArgs.ListingCustomTitle} {_listItArgs.ListingDefaultTitle}",
                Description = $"{_listItArgs.ListingCustomTitle} \r\n {_listItArgs.ListingDefaultDescription}",
                Quantity = _listItArgs.ListingDefaultQuantity,
            };
            listing.Price = decimal.TryParse(args[1], out decimal price) ? price : throw new InvalidDataException("Price must be a decimal value.");
            listing.IsSupply = true;
            listing.CategoryId = 69150433;
            listing.WhenMade = "2010_2017";
            listing.WhoMade = "i_did";
            listing.IsCustomizable = bool.TryParse(args[2], out bool isCustomizable) && isCustomizable;
            listing.IsDigital = true;
            listing.ShippingTemplateId = 30116314577;
            listing.Images = new[] { new ListingImage
                {
                    ImagePath = GetWatermarkedImagePath(workingDirectory),
                    Overwrite = true,
                    IsWatermarked = true,
                    Rank = 1}
            };
            listing.Tags = ParseTags(args);

            var zip = CreateZipFile(workingDirectory);
            listing.DigitalFiles = new[] { new DigitalFile()
                {
                    Path = zip,
                    Name = Path.GetFileName(zip),
                    Rank = 1}
            };

            var etsyListing = _etsyService.CreateListing(listing);
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
                    Console.Write("Enter your Etsy validator from the web browser to contine:");
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