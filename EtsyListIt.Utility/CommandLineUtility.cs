using EtsyListIt.Utility.DomainObjects;
using EtsyListIt.Utility.Extensions;
using EtsyListIt.Utility.Interfaces;
using NDesk.Options;

namespace EtsyListIt.Utility
{
    public class CommandLineUtility : ICommandLineUtility
    {
        private readonly ISettingsUtility _settingsHelper;

        public CommandLineUtility(ISettingsUtility settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }
        public EtsyListItArgs ParseCommandLineArguments(string[] args)
        {

            var commandLineArgs = new EtsyListItArgs();
            var p = new OptionSet()
            {
                {
                    "wd|Working Directory=", "Sets or changes the directory of the base file",
                    v => commandLineArgs.WorkingDirectory = v
                },
                {
                    "od|Output Directory=",
                    "Sets or changes the directory of the output files",
                    v => commandLineArgs.OutputDirectory = v
                },
                {
                    "a|APIKey=",
                    "Sets or changes the API key for the application",
                    v => commandLineArgs.APIKey = v
                },
                {
                    "ss|Shared Secret=",
                    "Sets or changes the shared secret for the application",
                    v => commandLineArgs.SharedSecret = v
                }
            };
            try
            {
                p.Parse(args);
                #region Working Directory
                if (commandLineArgs.WorkingDirectory.IsNullOrEmpty())
                {
                    commandLineArgs.WorkingDirectory = _settingsHelper.GetAppSetting("WorkingDirectory");
                    if (commandLineArgs.WorkingDirectory.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify working directory!  Use command line argument -wd {directory} to specify.");
                    }
                }
                else
                {
                    _settingsHelper.SetAppSetting("WorkingDirectory", commandLineArgs.WorkingDirectory);
                }
                #endregion
                #region Output Directory
                if (commandLineArgs.OutputDirectory.IsNullOrEmpty())
                {
                    commandLineArgs.OutputDirectory = _settingsHelper.GetAppSetting("OutputDirectory");
                    if (commandLineArgs.OutputDirectory.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify output directory!  Use command line argument -od {directory} to specify.");
                    }
                }
                else
                {
                    _settingsHelper.SetAppSetting("OutputDirectory", commandLineArgs.OutputDirectory);
                }
                #endregion
                #region API Key
                if (commandLineArgs.APIKey.IsNullOrEmpty())
                {
                    commandLineArgs.APIKey = _settingsHelper.GetEncryptedAppSetting("APIKey");
                    if (commandLineArgs.APIKey.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify the API Key for the application!  Use command line argument -a {key} to specify.");
                    }
                }
                else
                {
                    _settingsHelper.SetAppSettingWithEncryption("APIKey", commandLineArgs.APIKey);
                }
                #endregion
                #region Shared Secret
                if (commandLineArgs.SharedSecret.IsNullOrEmpty())
                {
                    commandLineArgs.SharedSecret = _settingsHelper.GetEncryptedAppSetting("SharedSecret");
                    if (commandLineArgs.SharedSecret.IsNullOrEmpty())
                    {
                        
                        throw new EtsyListItException("User must specify the Shared Secret for the application!  Use command line argument -ss {secret} to specify.");
                    }
                }
                else
                {
                    _settingsHelper.SetAppSettingWithEncryption("SharedSecret", commandLineArgs.SharedSecret);
                }
                #endregion
            }
            catch (OptionException e)
            {
                throw new EtsyListItException("Unable to parse commandLine args.  OptionsException: {0}".QuickFormat(e.Message));
            }
            return commandLineArgs;
        }
    }

}