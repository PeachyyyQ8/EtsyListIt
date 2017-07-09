namespace EtsyListIt.Utility.DomainObjects
{
    public class EtsyListItArgs
    {
        public string WorkingDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public string WatermarkFile { get; set; }
        public string APIKey { get; set; }
        public string SharedSecret { get; set; }
        public string ListingDefaultTitle { get; set; }
        public string ListingDefaultDescription { get; set; }
        public string ListingQuantity { get; set; }
        public string AddToEtsy { get; set; }
    }
}