namespace EtsyWrapper.DomainObjects
{
    public class DigitalListingWithImages : Listing
    {
        public string CategoryID { get; set; }
        public string WhenMade { get; set; }
        public string WhoMade { get; set; }
        public bool IsCustomizable { get; set; }
        public bool IsDigital { get; set; }
        public string ShippingTemplateID { get; set; }
        public ListingImage[] Images { get; set; }
        public DigitalFile[] DigitalFiles { get; set; }
        public string[] Tags { get; set; }
    }
}