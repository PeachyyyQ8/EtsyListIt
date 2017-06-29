using System.Collections.Generic;

namespace EtsyListIt.Utility.Interfaces
{
    public interface ISystemUtility
    {
        string CreateZipFileFromDirectory(string baseFile, string outputDirectory);
        void DeleteFilesInDirectory(string outputDirectory, List<string> ignoredContent = null);
    }
}