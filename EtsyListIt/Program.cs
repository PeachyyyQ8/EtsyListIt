using System;
using System.Diagnostics;
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

        static void Main(string[] args)
        {
            try
            {
                #region IOC

                _container = ConfigureStructureMap();
                var settingsUtility = _container.GetInstance<ISettingsUtility>();
                var commandLineUtility = _container.GetInstance<ICommandLineUtility>();

                #endregion

                var listItArgs = commandLineUtility.ParseCommandLineArguments(args);

                var authToken = GetAuthToken(listItArgs, settingsUtility);


                if (!authToken.IsValidEtsyToken())
                {
                    throw new EtsyListItException(
                        "Invalid AuthToken.  Please use the correct Verifier key obtained from Etsy to generate your permanent token credentials or specify them with command line args." +
                        "Permanent Token: -pt [value]" +
                        "Permanent Secret: -ps [value]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Write("Type any key to quit.");
                Console.ReadLine();
            }
        }

        private static Container ConfigureStructureMap()
        {
            return new Container(new DependencyRegistry());
        }

        private static PermanentToken GetAuthToken(EtsyListItArgs args, ISettingsUtility settingsUtility)
        {
            var permanentToken = new PermanentToken();
            // First check command line.  If command line args not found, check settings.
            if (args.APIKey.IsNullOrEmpty())
            {
                permanentToken.APIKey = settingsUtility.GetAppSetting("APIKey");
            }
            if (args.SharedSecret.IsNullOrEmpty())
            {
                permanentToken.SharedSecret = settingsUtility.GetAppSetting("SharedSecret");
            }
            if (args.PermanentAuthToken.IsNullOrEmpty())
            {
                permanentToken.TokenID = settingsUtility.GetAppSetting("PermanentAuthToken");
            }
            if (args.PermanentSecret.IsNullOrEmpty())
            {
                permanentToken.TokenSecret = settingsUtility.GetAppSetting("PermanentSecret");
            }

            // If API Key or Shared Secret not found either place, throw error.
            if (permanentToken.APIKey.IsNullOrEmpty() || permanentToken.SharedSecret.IsNullOrEmpty())
            {
                throw new EtsyListItException(
                    "Must specify API Key and Shared Secret to authenticate.  Please input command line arg -a [value] for API Key and -ss [value] for SharedSecret to continue.");
            }

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
                    authenticationWrapper.GetPermanentTokenCredentials(args.APIKey, args.SharedSecret, validator,
                        permissions);
            }

            return permanentToken;
        }
    }
}