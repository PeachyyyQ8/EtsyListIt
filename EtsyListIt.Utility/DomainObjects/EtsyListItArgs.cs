namespace EtsyListIt.Utility.DomainObjects
{
    public class EtsyListItArgs
    {
        public string WorkingDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public string WatermarkFile { get; set; }
        public string APIKey { get; set; }
        public string SharedSecret { get; set; }
        public string PermanentAuthToken { get; set; }
        public string PermanentSecret { get; set; }
    }
}