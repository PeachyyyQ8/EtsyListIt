using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtsyListIt.Utility.Interfaces
{
    public interface ISettingsUtility
    {
        object GetAppSetting(string outputdirectory);
        void SetAppSetting(string key, string workingDirectory);
    }
}
