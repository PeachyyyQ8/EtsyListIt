using EtsyListIt.Utility.DomainObjects;
using EtsyListIt.Utility.Extensions;
using EtsyListIt.Utility.Interfaces;
using NDesk.Options;

namespace EtsyListIt.Utility
{
    public class CommandLineUtility : ICommandLineUtility
    {
        private readonly ISettingsUtility _settingsHelper;
        private EtsyListItArgs _commandLineArgs;

        public CommandLineUtility(ISettingsUtility settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }
        public EtsyListItArgs ParseCommandLineArguments(string[] args)
        {
            _commandLineArgs = new EtsyListItArgs();
            var p = new OptionSet()
            {
                {
                    "wd|Working Directory=", "Sets or changes the directory of the base file",
                    v => _commandLineArgs.WorkingDirectory = v
                },
                {
                    "od|Output Directory=",
                    "Sets or changes the directory of the output files",
                    v => _commandLineArgs.OutputDirectory = v
                },
                {
                    "a|APIKey=",
                    "Sets or changes the API key for the application",
                    v => _commandLineArgs.APIKey = v
                },
                {
                    "ss|Shared Secret=",
                    "Sets or changes the shared secret for the application",
                    v => _commandLineArgs.SharedSecret = v
                },
                {
                    "dt|Default Title=",
                    "Sets or changes the default title for the listing.",
                    v => _commandLineArgs.ListingDefaultTitle = v
                },
                {
                    "dd|Default Description=",
                    "Sets or changes the default description for the listing.",
                    v => _commandLineArgs.ListingDefaultDescription = v
                },
                {
                    "lq|Listing Quantity=",
                    "Sets or changes the quantity for the listing.",
                    v => _commandLineArgs.ListingQuantity = v
                },
                {
                "wm|Watermark File=" +
                "",
                "Sets the watermark to place on the graphic.",
                v => _commandLineArgs.WatermarkFile = v
            },
                {
                    "update|Update Listing=" + 
                    "",
                    "Tells the application whether you want to add a listing for the graphic to Etsy.",
                    v => _commandLineArgs.UpdateListingID = v
                },
                {
                    "add|Add To Etsy=" +
                    "",
                    "Tells the application whether you want to add a listing for the graphic to Etsy.",
                    v => _commandLineArgs.AddToEtsy = v
                },
                {
                "publish|Publish Listing=" +
                "",
                "Tells the application whether you want to publish the listing.",
                v => _commandLineArgs.PublishListing = v
            }




            };
            try
            {
                p.Parse(args);

                _commandLineArgs.WorkingDirectory = GetValueAndStore("WorkingDirectory", _commandLineArgs.WorkingDirectory, "-wd");
                _commandLineArgs.OutputDirectory =
                    GetValueAndStore("Output Directory", _commandLineArgs.OutputDirectory, "-od");
                _commandLineArgs.APIKey = GetValueAndStore("APIKey", _commandLineArgs.APIKey, "-a");
                _commandLineArgs.SharedSecret = GetValueAndStore("SharedSecret", _commandLineArgs.SharedSecret, "-ss");
                _commandLineArgs.ListingDefaultTitle =
                    GetValueAndStore("ListingDefaultTitle", _commandLineArgs.ListingDefaultTitle, "-dt");
                _commandLineArgs.ListingDefaultDescription =
                    GetValueAndStore("ListingDefaultDescription", _commandLineArgs.ListingDefaultDescription, "-dd");
                _commandLineArgs.ListingQuantity = GetValueAndStore("ListingQuantity",
                    _commandLineArgs.ListingQuantity, "-lq");
                _commandLineArgs.WatermarkFile = GetValueAndStore("WatermarkFile",
                    _commandLineArgs.WatermarkFile, "-wm");
                _commandLineArgs.PublishListing = GetValueAndStore("PublishListing",
                    _commandLineArgs.WatermarkFile, "-publish");
                _commandLineArgs.AddToEtsy = GetValueAndStore("AddToEtsy", _commandLineArgs.AddToEtsy, "-add");
            }
            catch (OptionException e)
            {
                throw new EtsyListItException("Unable to parse commandLine args.  OptionsException: {0}".QuickFormat(e.Message));
            }
            return _commandLineArgs;
        }

        private string GetValueAndStore(string key, string value, string argName)
        {
            if (value.IsNullOrEmpty())
            {
                value = _settingsHelper.GetAppSetting(key);
                if (value.IsNullOrEmpty())
                {
                    throw new EtsyListItException(
                        $"User must specify {key}!  Use command line argument {argName} value to specify.");
                }
            }
            else
            {
                _settingsHelper.SetAppSetting(key, value);
            }

            return value;
        }
    }

}