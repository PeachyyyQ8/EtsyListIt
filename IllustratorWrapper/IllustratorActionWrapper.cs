using System;
using System.IO;
using Illustrator;

namespace IllustratorWrapper
{
    public class IllustratorActionWrapper : IIllustratorActionWrapper
    {
        private readonly dynamic _application;

        public IllustratorActionWrapper()
        {
            var type = Type.GetTypeFromProgID("Illustrator.Application");
            _application = Activator.CreateInstance(type);
        }
        public void ExportFileAsJPEG(string baseFile, string outputDirectory, string newFileName = null)
        {
            var fullFileName = GetFullFileName(baseFile, outputDirectory, newFileName, ".jpg");
            
            dynamic document = _application.Open(baseFile);
            FitArtboardToCurrentDocument(document);
            //CenterItemsOnArtboard(document);
            dynamic jpgOptions = new ExportOptionsJPEG();
            jpgOptions.QualitySetting = 100;
            jpgOptions.AntiAliasing = false;
            jpgOptions.Optimization = false;
            document.Export(fullFileName, AiExportType.aiJPEG, jpgOptions);
            document.Close(AiSaveOptions.aiDoNotSaveChanges);
        }
        
        public void ExportFileAsDXF(string baseFile, string outputDirectory, string extension, string newFileName = null)
        {
            var fullFileName = GetFullFileName(baseFile, outputDirectory, newFileName, extension);
            
            dynamic document = _application.Open(baseFile);
            FitArtboardToCurrentDocument(document);
            //CenterItemsOnArtboard(document);
            dynamic dxfOptions = new ExportOptionsAutoCAD();
            Type enumType = typeof(AiAutoCADExportFileFormat);
            dynamic enumValue = enumType.GetField("aiDXF").GetValue(null);
            dxfOptions.ExportFileFormat = enumValue;
            document.Export(fullFileName, AiExportType.aiAutoCAD, dxfOptions);
            document.Close(AiSaveOptions.aiDoNotSaveChanges);
        }

        public void SaveFileAsEPS(string baseFile, string outputDirectory, string newFileName = null)
        {
            var fullFileName = GetFullFileName(baseFile, outputDirectory, newFileName, ".eps");

            dynamic document = _application.Open(baseFile);
            FitArtboardToCurrentDocument(document);
            //CenterItemsOnArtboard(document);
            document.SaveAs(fullFileName, new EPSSaveOptions());
            document.Close(AiSaveOptions.aiDoNotSaveChanges);
        }

        public void SaveFileAsPDF(string baseFile, string outputDirectory, string newFileName = null)
        {
            var fullFileName = GetFullFileName(baseFile, outputDirectory, newFileName, ".pdf");

            dynamic document = _application.Open(baseFile);
            FitArtboardToCurrentDocument(document);
            //CenterItemsOnArtboard(document);
            document.SaveAs(fullFileName, new PDFSaveOptions());
            document.Close(AiSaveOptions.aiDoNotSaveChanges);
        }

        public string SaveFileWithWatermark(string watermarkFile, string baseFile, string outputDirectory)
        {

            var fullFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(baseFile) + "Watermarked.jpg");

            dynamic sourceDocument = _application.Open(baseFile);
            FitArtboardToCurrentDocument(sourceDocument);

            //select all and group
            sourceDocument.SelectObjectsOnActiveArtboard();
            dynamic sourceDocSelection = sourceDocument.Selection;
            if (sourceDocSelection.Length == 0)
            {
                throw new IllustratorException("Selected image is empty!");
            }
            dynamic imageGroup = sourceDocument.groupItems.add();
            for (var i = 0; i < sourceDocSelection.Length; i++)
            {
                sourceDocSelection[i].moveToBeginning(imageGroup);
            }

            var imageHeight = imageGroup.Height;
            var imageWidth = imageGroup.Width;
            
            // select all and group
            dynamic watermarkDocument = _application.Open(watermarkFile,
                AiDocumentColorSpace.aiDocumentRGBColor, null);
            watermarkDocument.SelectObjectsOnActiveArtboard();
            dynamic watermarkDocSelection = watermarkDocument.Selection;
            if (watermarkDocSelection.Length == 0)
            {
                throw new IllustratorException("Selected image is empty!");
            }
            dynamic watermarkGroup = watermarkDocument.groupItems.add();
            for (var i = 0; i < watermarkDocSelection.Length; i++)
            {
                watermarkDocSelection[i].moveToBeginning(watermarkGroup);
            }

            //scale watermark to fit graphic
            int scale = 0;

            var watermarkHeight = watermarkGroup.Height;
            var watermarkWidth = watermarkGroup.Width;

            if (imageHeight > imageWidth)
            {
                scale = int.Parse(Math.Round(imageHeight / watermarkHeight * 100).ToString()) + 5;
            }
            else
            {
                scale = int.Parse(Math.Round(imageWidth / watermarkWidth * 100).ToString()) + 5;
            }
            watermarkGroup.Resize(scale, scale);

            // copy watermark to grpahic
            watermarkDocument.Copy();
            sourceDocument.Paste();

            watermarkDocument.Close(AiSaveOptions.aiDoNotSaveChanges);
            CenterItemsOnArtboard(sourceDocument);

            dynamic jpgOptions = new ExportOptionsJPEG();
            jpgOptions.QualitySetting = 100;
            jpgOptions.AntiAliasing = false;
            jpgOptions.Optimization = false;
            sourceDocument.Export(fullFileName, AiExportType.aiJPEG, jpgOptions);
            sourceDocument.Close(AiSaveOptions.aiDoNotSaveChanges);

            return fullFileName;
        }

        private string GetFullFileName(string baseFile, string outputDirectory, string newFileName, string extension)
        {
            if (newFileName != null)
            {
                if (!newFileName.Contains(extension))
                {
                    newFileName = newFileName + extension;
                }
                return Path.Combine(outputDirectory, newFileName);
            }
            else
            {
                return Path.Combine(outputDirectory,
                    Path.GetFileNameWithoutExtension(baseFile) + extension);
            }
        }

        public void ExportFileAsPNG(string baseFile, string outputDirectory, string newFileName = null)
        {
            var fullFileName = GetFullFileName(baseFile, outputDirectory, newFileName, ".png");
            
            dynamic document = _application.Open(baseFile);
            FitArtboardToCurrentDocument(document);
            //CenterItemsOnArtboard(document);
            dynamic pngOptions = new ExportOptionsPNG24();
            pngOptions.Transparency = true;
            pngOptions.AntiAliasing = false;
            pngOptions.SaveAsHTML = false;
            document.Export(fullFileName, AiExportType.aiPNG24, pngOptions);
            document.Close(AiSaveOptions.aiDoNotSaveChanges);
        }

        public void CenterItemsOnArtboard(dynamic document)
        {
            document.SelectObjectsOnActiveArtboard();
            var sel = document.Selection;

            if (sel.Length > 0)
            {
                foreach (var s in sel)
                {
                    dynamic objectToCenter = s;
                    var artboardIndex = document.Artboards.GetActiveArtboardIndex() + 1;
                    var activeArtboard = document.Artboards[artboardIndex].ArtboardRect;
                    var vertCenter = activeArtboard[1] / 2;
                    var horizCenter = activeArtboard[2] / 2;

                    var left = horizCenter - (objectToCenter.Width / 2);
                    var top = vertCenter + (objectToCenter.Height / 2);
                    
                    dynamic newPosition = new[]
                    {
                        left, top
                    };

                    objectToCenter.position = newPosition;
                }
            }
            else
            {
                throw new IllustratorException("There is no selection!");
            }
        }

        public void FitArtboardToCurrentDocument(dynamic document)
        {
            var index = document.Artboards.GetActiveArtboardIndex() + 1;
            var left = document.VisibleBounds[0];
            var top = document.VisibleBounds[1];
            var right = document.VisibleBounds[2];
            var bottom = document.VisibleBounds[3];
            document.Artboards[index].ArtboardRect = new[]
            {
                document.VisibleBounds[0] - document.VisibleBounds[1] * .05,
                document.VisibleBounds[1] + document.VisibleBounds[1] * .05,
                document.VisibleBounds[2] + document.VisibleBounds[1] * .05,
                document.VisibleBounds[3] - document.VisibleBounds[1] * .05
            };
        }

        public void UngroupItems(dynamic groupItems)
        {
            foreach (dynamic item in groupItems)
            {
                groupItems.Remove(item);
            }
        }

        public void ExportAll(string baseFile, string tempDirectoryPath)
        {
            throw new NotImplementedException();
        }

        public bool FileHasMultipleArtboards(string baseFile)
        {
            throw new NotImplementedException();
        }

        public void ExportMultipleArtboards(string baseFile)
        {
            throw new NotImplementedException();
        }

        public dynamic GroupItems(dynamic document)
        {
            document.SelectObjectsOnActiveArtboard();
            dynamic selection = document.Selection;
            dynamic imageGroup = document.GroupItems.Add();
            for (var i = 0; i < selection.Length; i++)
            {
                selection[i].moveToBeginning(imageGroup);
            }

            return document.GroupItems;
        }
    }
}