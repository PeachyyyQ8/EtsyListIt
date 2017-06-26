namespace EtsyWrapper.DomainObjects
{
    public class ListingImage
    {
        public string ImagePath { get; set; }
        public bool Overwrite { get; set; }
        public bool IsWatermarked { get; set; }
        public int Rank { get; set; }
    }
}