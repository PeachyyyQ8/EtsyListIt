using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using EtsyListIt.Utility.Extensions;
using EtsyListIt.Utility.Interfaces;

namespace EtsyListIt.Utility
{
    public class SystemUtility : ISystemUtility
    {
        public string CreateZipFileFromDirectory(string baseFile, string outputDirectory)
        {
            var files = Directory.GetFiles(outputDirectory);
            var zipFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(baseFile) + ".zip");
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                }
            }

            return zipFileName;
        }

        public void DeleteFilesInDirectory(string outputDirectory, List<string> ignoredContent = null)
        {
            foreach (var file in Directory.GetFiles(outputDirectory))
            {
             
                if (!ignoredContent.IsNullOrEmpty()) continue;
                // ReSharper disable once AssignNullToNotNullAttribute
                var ignoreFile = ignoredContent.Any(ignored => file.Contains(ignored));
                if (ignoreFile)
                {
                    continue;
                }

                File.Delete(file);
            }
        }
    }
}