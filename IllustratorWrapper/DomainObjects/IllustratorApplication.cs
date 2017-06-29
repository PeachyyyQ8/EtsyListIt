using System;
using Illustrator;

namespace IllustratorWrapper.DomainObjects
{
    public class IllustratorApplication : Application
    {
        private dynamic _application;
        public IllustratorApplication()
        {
            Type type = Type.GetTypeFromProgID("Illustrator.Application");
            _application = Activator.CreateInstance(type);
        }

        public void executeAATFile(string File)
        {
            throw new NotImplementedException();
        }

        public void Cut()
        {
            throw new NotImplementedException();
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public bool IsFillActive()
        {
            throw new NotImplementedException();
        }

        public bool IsStrokeActive()
        {
            throw new NotImplementedException();
        }

        public object ShowColorPicker(object Color)
        {
            throw new NotImplementedException();
        }

        public Document OpenCloudLibraryAssetForEditing(string AssetURL, string ThumbnailURL, string AssetType, object Options = null)
        {
            throw new NotImplementedException();
        }

        public void SetThumbnailOptionsForCloudLibrary(object Options)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void DoScript(string Action, string From, object Dialogs = null)
        {
            throw new NotImplementedException();
        }

        public void ExecuteMenuCommand(string MenuCommandString)
        {
            throw new NotImplementedException();
        }

        public string GetPresetFileOfType(AiDocumentPresetType PresetType)
        {
            throw new NotImplementedException();
        }

        public void LoadAction(string ActionFilePath)
        {
            throw new NotImplementedException();
        }

        public void UnloadAction(string SetName, string ActionName)
        {
            throw new NotImplementedException();
        }

        public string SendScriptMessage(string PluginName, string MessageSelector, string InputString)
        {
            throw new NotImplementedException();
        }

        public DocumentPreset GetPresetSettings(string Preset)
        {
            throw new NotImplementedException();
        }

        public void Redraw()
        {
            throw new NotImplementedException();
        }

        public Matrix InvertMatrix(Matrix Matrix)
        {
            throw new NotImplementedException();
        }

        public bool IsSingularMatrix(Matrix Matrix)
        {
            throw new NotImplementedException();
        }

        public Matrix ConcatenateTranslationMatrix(Matrix Matrix, object DeltaX = null, object DeltaY = null)
        {
            throw new NotImplementedException();
        }

        public Matrix ConcatenateScaleMatrix(Matrix Matrix, object ScaleX = null, object ScaleY = null)
        {
            throw new NotImplementedException();
        }

        public Matrix ConcatenateRotationMatrix(Matrix Matrix, double Angle)
        {
            throw new NotImplementedException();
        }

        public Matrix ConcatenateMatrix(Matrix Matrix, Matrix SecondMatrix)
        {
            throw new NotImplementedException();
        }

        public bool IsEqualMatrix(Matrix Matrix, Matrix SecondMatrix)
        {
            throw new NotImplementedException();
        }

        public Matrix GetIdentityMatrix()
        {
            throw new NotImplementedException();
        }

        public Matrix GetRotationMatrix(object Angle = null)
        {
            throw new NotImplementedException();
        }

        public Matrix GetScaleMatrix(object ScaleX = null, object ScaleY = null)
        {
            throw new NotImplementedException();
        }

        public Matrix GetTranslationMatrix(object DeltaX = null, object DeltaY = null)
        {
            throw new NotImplementedException();
        }

        public Document Open(string File, object DocumentColorSpace = null, object Options = null)
        {
            throw new NotImplementedException();
        }

        public void Quit()
        {
            throw new NotImplementedException();
        }

        public string DoJavaScript(string JavaScriptCode, object Arguments = null, object ExecutionMode = null)
        {
            throw new NotImplementedException();
        }

        public string DoJavaScriptFile(string JavaScriptFile, object Arguments = null, object ExecutionMode = null)
        {
            throw new NotImplementedException();
        }

        public string TranslatePlaceholderText(string Text)
        {
            throw new NotImplementedException();
        }

        public object ShowPresets(string FileSpec)
        {
            throw new NotImplementedException();
        }

        public void LoadColorSettings(string FileSpec)
        {
            throw new NotImplementedException();
        }

        public PPDFileInfo GetPPDFileInfo(string Name)
        {
            throw new NotImplementedException();
        }

        public object GetScriptableHelpGroup()
        {
            throw new NotImplementedException();
        }

        public object ConvertSampleColor(AiImageColorSpace SourceColorSpace, object SourceColor, AiImageColorSpace DestColorSpace,
            AiColorConvertPurpose ColorConvertPurpose, object SourceHasAlpha = null, object DestHasAlpha = null)
        {
            throw new NotImplementedException();
        }

        public void ReflectCSAW(string OutputFolder)
        {
            throw new NotImplementedException();
        }

        public bool SwitchWorkspace(string WorkspaceName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteWorkspace(string WorkspaceName)
        {
            throw new NotImplementedException();
        }

        public bool SaveWorkspace(string WorkspaceName)
        {
            throw new NotImplementedException();
        }

        public bool ResetWorkspace()
        {
            throw new NotImplementedException();
        }

        public bool IsTouchWorkspace()
        {
            throw new NotImplementedException();
        }

        public bool IsUserSharingAppUsageData()
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public string Path { get; }
        public Document ActiveDocument { get; set; }
        public string UserAdobeID { get; }
        public string UserGUID { get; }
        public string Version { get; }
        public string BuildNumber { get; }
        public string Locale { get; }
        public string ScriptingVersion { get; }
        public int FreeMemory { get; }
        public bool BrowserAvailable { get; }
        public object Selection { get; set; }
        public bool Visible { get; }
        public AiUserInteractionLevel UserInteractionLevel { get; set; }
        public AiCoordinateSystem CoordinateSystem { get; set; }
        public bool ActionIsRunning { get; }
        public Preferences Preferences { get; }
        public object PrinterList { get; }
        public object PPDFileList { get; }
        public object PrintPresetsList { get; }
        public object StartupPresetsList { get; }
        public object PDFPresetsList { get; }
        public object FlattenerPresetsList { get; }
        public object TracingPresetsList { get; }
        public object ColorSettingsList { get; }
        public string DefaultColorSettings { get; }
        public bool PasteRemembersLayers { get; set; }
        public Documents Documents { get; }
        public TextFonts TextFonts { get; }
        public Application Application { get; }
    }
}