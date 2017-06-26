using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtsyListIt.Utility.Interfaces
{
    public interface ISettingsUtility
    {
        string GetAppSetting(string settingKey);
        void SetAppSetting(string key, string workingDirectory);
    }
}
