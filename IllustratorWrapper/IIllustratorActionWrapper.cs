using System.Collections.Generic;
using Illustrator;

namespace IllustratorWrapper
{
    public interface IIllustratorActionWrapper
    {
        dynamic GroupItems(dynamic document);
        string CreateWatermarkForArtboard(string baseFile, string tempDirectoryPath, string watermarkPath, int i);
        void ExportFileAsJPEG(string baseFile, string outputDirectory, string newFileName = null);
        void ExportFileAsDXF(string baseFile, string outputDirectory, string extension, string newFileName = null);
        void SaveFileAsEPS(string baseFile, string outputDirectory, string newFileName = null);
        void SaveFileAsPDF(string baseFile, string outputDirectory, string newFileName = null);
        void ExportFileAsSVG(string baseFile, string outputDirectory, string newFileName = null);
        string SaveFileWithWatermark(string watermarkFile, string baseFile, string outputDirectory);
        void ExportFileAsPNG(string baseFile, string outputDirectory, string newFileName = null);
        void CenterItemsOnArtboard(dynamic document);
        void FitArtboardToCurrentDocument(dynamic document);
        void UngroupItems(dynamic groupItems);
        void ExportAll(string baseFile, string tempDirectoryPath, string newFileName = null);
        void ExportMultipleArtboards(string baseFile, string tempDirectoryPath);
        int CountArtboards(string baseFile);
    }
}