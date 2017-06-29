namespace IllustratorWrapper
{
    public interface IIllustratorActionWrapper
    {
        void ExportFileAsJPEG(string baseFile, string outputDirectory, string newFileName = null);
        void ExportFileAsPNG(string baseFile, string outputDirectory, string newFileName = null);
        void ExportFileAsDXF(string baseFile, string outputDirectory, string newFileName = null);
        void SaveFileAsEPS(string baseFile, string outputDirectory, string newFileName = null);
        void SaveFileAsPDF(string baseFile, string outputDirectory, string newFileName = null);
        string SaveFileWithWatermark(string watermarkFile, string baseFile, string outputDirectory);
       void CenterItemsOnArtboard(dynamic aDoc);
    }
}