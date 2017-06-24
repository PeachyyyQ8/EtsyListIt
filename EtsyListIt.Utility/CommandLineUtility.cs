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
            var p = new OptionSet() {
                { "wd|Working Directory=", "Sets or changes the directory of the base file.",
                    v => commandLineArgs.WorkingDirectory = v },
                { "od|Output Directory=",
                    "Sets or changes the directory of the output files",
                    v => commandLineArgs.OutputDirectory = v},
                { "wm|Watermark File=",
                    "Sets or changes the path of the watermark files",
                    v => commandLineArgs.WatermarkFile = v}
            };
            try
            {
                p.Parse(args);
                if (commandLineArgs.WorkingDirectory.IsNullOrEmpty())
                {
                    var workingDirectory = _settingsHelper.GetAppSetting("workingDirectory").ToString();
                    if (workingDirectory.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify working directory!  Use -wd {directory} to specify.");
                    }

                    _settingsHelper.SetAppSetting("workingDirectory", workingDirectory);
                }

                if (commandLineArgs.OutputDirectory.IsNullOrEmpty())
                {
                    var outputDirectory = _settingsHelper.GetAppSetting("outputDirectory").ToString();
                    if (outputDirectory.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify output directory!  Use -od {directory} to specify.");
                    }

                    _settingsHelper.SetAppSetting("outputDirectory", outputDirectory);
                }

                if (commandLineArgs.WatermarkFile.IsNullOrEmpty())
                {
                    var watermarkFile = _settingsHelper.GetAppSetting("watermarkFile").ToString();
                    if (watermarkFile.IsNullOrEmpty())
                    {
                        throw new EtsyListItException("User must specify watermark file!  Use -wm {filePath} to specify.");
                    }

                    _settingsHelper.SetAppSetting("watermarkFile", watermarkFile);
                }
            }
            catch (OptionException e)
            {
                throw new EtsyListItException("Unable to parse commandLine args.  OptionsException: {0}".QuickFormat(e.Message));
            }
            return commandLineArgs;
        }
    }

}