using System.IO;
using Illustrator;
using IllustratorWrapper.DomainObjects;
using IllustratorWrapper.Interfaces;

namespace IllustratorWrapper
{
    public class IllustratorDocumentWrapper : IIllustratorDocumentWrapper
    {
        private readonly IIllustratorApplicationWrapper _illustratorApplicationWrapper;

        public IllustratorDocumentWrapper(IIllustratorApplicationWrapper illustratorApplicationWrapper)
        {
            _illustratorApplicationWrapper = illustratorApplicationWrapper;
        }

        
        public void ExportFileAsJPEG(string baseFile, string outputDirectory, int qualitySetting = 30, string fileName = null)
        {
            var exportedFileName = fileName ?? Path.GetFileNameWithoutExtension(baseFile) + ".jpg";
            var document = _illustratorApplicationWrapper.Open(baseFile, AiDocumentColorSpace.aiDocumentRGBColor);
            var fullFileName = Path.Combine(outputDirectory, exportedFileName);
            dynamic jpgOptions = new ExportOptionsJPEG();
            jpgOptions.QualitySetting = qualitySetting;
            jpgOptions.AntiAliasing = false;
            jpgOptions.Optimization = false;
            document.Export(fullFileName, AiExportType.aiJPEG, jpgOptions);
        }
    }
}