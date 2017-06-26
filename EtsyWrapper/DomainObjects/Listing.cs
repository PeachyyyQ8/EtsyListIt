namespace EtsyWrapper.DomainObjects
{
    public class Listing
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsSupply { get; set; }
    }
}