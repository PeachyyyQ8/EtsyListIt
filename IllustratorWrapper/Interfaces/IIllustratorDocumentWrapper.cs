namespace IllustratorWrapper.Interfaces
{
    public interface IIllustratorDocumentWrapper
    {
        void ExportFileAsJPEG(string baseFile, string outputDirectory, int qualitySetting = 30, string fileName = null);
    }
}