using Illustrator;

namespace IllustratorWrapper.DomainObjects
{
    public class IllustratorDocument : Document
    {
        public void let_DefaultFillColor(object o)
        {
            throw new System.NotImplementedException();
        }

        public void let_DefaultStrokeColor(object o)
        {
            throw new System.NotImplementedException();
        }

        public void let_RasterEffectSettings(RasterEffectOptions options)
        {
            throw new System.NotImplementedException();
        }

        public void Close(object Saving = null)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessGesture(string GesturePointsFile)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void SaveAs(string SaveIn, object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public void PrintOut(object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public void Export(string ExportFile, AiExportType ExportFormat, object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public void ExportSelectionAsPNG(string ExportFile, object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public void ExportSelectedArtwork(string ExportFile)
        {
            throw new System.NotImplementedException();
        }

        public void ImportFileIntoDocument(string ImportFile, bool IsLinked, object LibraryName = null, object ItemName = null,
            object ElementRef = null, object ModifiedTime = null, object CreationTime = null, object AdobeStockId = null,
            object AdobeStockLicense = null)
        {
            throw new System.NotImplementedException();
        }

        public void Cut()
        {
            throw new System.NotImplementedException();
        }

        public void Copy()
        {
            throw new System.NotImplementedException();
        }

        public void Paste()
        {
            throw new System.NotImplementedException();
        }

        public void Activate()
        {
            throw new System.NotImplementedException();
        }

        public void ImportVariables(string FileSpec)
        {
            throw new System.NotImplementedException();
        }

        public void ExportVariables(string File)
        {
            throw new System.NotImplementedException();
        }

        public void ImportCharacterStyles(string FileSpec)
        {
            throw new System.NotImplementedException();
        }

        public void ImportParagraphStyles(string FileSpec)
        {
            throw new System.NotImplementedException();
        }

        public void ImportPrintPreset(string PrintPreset, string FileSpec)
        {
            throw new System.NotImplementedException();
        }

        public void ExportPrintPreset(string File)
        {
            throw new System.NotImplementedException();
        }

        public void ImportPDFPreset(string FileSpec, object ReplacingPreset = null)
        {
            throw new System.NotImplementedException();
        }

        public void ExportPDFPreset(string File)
        {
            throw new System.NotImplementedException();
        }

        public void ImportPerspectiveGridPreset(string FileSpec, object PerspectivePreset = null)
        {
            throw new System.NotImplementedException();
        }

        public void ExportPerspectiveGridPreset(string File)
        {
            throw new System.NotImplementedException();
        }

        public void ImageCapture(string ImageFile, object ClipBounds = null, object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public void WindowCapture(string ImageFile, object WindowSize)
        {
            throw new System.NotImplementedException();
        }

        public object Rasterize(object SourceArt, object ClipBounds = null, object Options = null)
        {
            throw new System.NotImplementedException();
        }

        public bool RearrangeArtboards(object ArtboardLayout = null, object ArtboardRowsOrCols = null, object ArtboardSpacing = null,
            object ArtboardMoveArtwork = null)
        {
            throw new System.NotImplementedException();
        }

        public bool SelectObjectsOnActiveArtboard()
        {
            throw new System.NotImplementedException();
        }

        public bool FitArtboardToSelectedArt(object Index = null)
        {
            throw new System.NotImplementedException();
        }

        public object ConvertCoordinate(object Coordinate, AiCoordinateSystem Source, AiCoordinateSystem Destination)
        {
            throw new System.NotImplementedException();
        }

        public bool SelectPerspectivePreset(string PerspectivePreset)
        {
            throw new System.NotImplementedException();
        }

        public bool ShowPerspectiveGrid()
        {
            throw new System.NotImplementedException();
        }

        public bool HidePerspectiveGrid()
        {
            throw new System.NotImplementedException();
        }

        public AiPerspectiveGridPlaneType GetPerspectiveActivePlane()
        {
            throw new System.NotImplementedException();
        }

        public bool SetPerspectiveActivePlane(AiPerspectiveGridPlaneType PerspectiveGridPlane)
        {
            throw new System.NotImplementedException();
        }

        public bool Stationery { get; }
        public string FullName { get; }
        public Layer ActiveLayer { get; set; }
        public View ActiveView { get; }
        public object GeometricBounds { get; }
        public object VisibleBounds { get; }
        public object RulerOrigin { get; set; }
        public AiRulerUnits RulerUnits { get; }
        public object PageOrigin { get; set; }
        public object CropBox { get; set; }
        public AiCropOptions CropStyle { get; set; }
        public double Width { get; }
        public double Height { get; }
        public bool ShowPlacedImages { get; }
        public double OutputResolution { get; }
        public bool PrintTiles { get; }
        public object Selection { get; set; }
        public bool SplitLongPaths { get; }
        public bool TileFullPages { get; }
        public bool UseDefaultScreen { get; }
        public AiDocumentColorSpace DocumentColorSpace { get; }
        public string Name { get; }
        public string Path { get; }
        public bool Saved { get; set; }
        public bool DefaultFilled { get; set; }
        public object DefaultFillColor { get; set; }
        public bool DefaultFillOverprint { get; set; }
        public bool DefaultStroked { get; set; }
        public object DefaultStrokeColor { get; set; }
        public bool DefaultStrokeOverprint { get; set; }
        public double DefaultStrokeWidth { get; set; }
        public object DefaultStrokeDashes { get; set; }
        public double DefaultStrokeDashOffset { get; set; }
        public AiStrokeCap DefaultStrokeCap { get; set; }
        public AiStrokeJoin DefaultStrokeJoin { get; set; }
        public double DefaultStrokeMiterLimit { get; set; }
        public DataSet ActiveDataSet { get; set; }
        public bool VariablesLocked { get; set; }
        public string XMPString { get; set; }
        public RasterEffectOptions RasterEffectSettings { get; set; }
        public string ColorProfileName { get; }
        public Artboards Artboards { get; }
        public CompoundPathItems CompoundPathItems { get; }
        public Layers Layers { get; }
        public PageItems PageItems { get; }
        public PathItems PathItems { get; }
        public Tags Tags { get; }
        public Views Views { get; }
        public RasterItems RasterItems { get; }
        public PlacedItems PlacedItems { get; }
        public EmbeddedItems EmbeddedItems { get; }
        public MeshItems MeshItems { get; }
        public PluginItems PluginItems { get; }
        public GraphItems GraphItems { get; }
        public NonNativeItems NonNativeItems { get; }
        public GroupItems GroupItems { get; }
        public TextFrames TextFrames { get; }
        public Stories Stories { get; }
        public CharacterStyles CharacterStyles { get; }
        public ParagraphStyles ParagraphStyles { get; }
        public object KinsokuSet { get; }
        public object MojikumiSet { get; }
        public Swatches Swatches { get; }
        public SwatchGroups SwatchGroups { get; }
        public Gradients Gradients { get; }
        public Patterns Patterns { get; }
        public Spots Spots { get; }
        public Symbols Symbols { get; }
        public SymbolItems SymbolItems { get; }
        public Brushes Brushes { get; }
        public GraphicStyles GraphicStyles { get; }
        public Variables Variables { get; }
        public object InkList { get; }
        public DataSets DataSets { get; }
        public LegacyTextItems LegacyTextItems { get; }
        public Application Application { get; }
        public object Parent { get; }
    }
}