using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;

namespace EtsyWrapper
{
    public class ListingWrapper : IListingWrapper
    {
        public Listing CreateListing(Listing listing)
        {
            throw new System.NotImplementedException();
        }

        public Listing CreateListingWithImage(ListingWithImage listing)
        {
            throw new System.NotImplementedException();
        }

        public DigitalListingWithImages CreateDigitalListingWithImage(DigitalListingWithImages digitalListingWithImages)
        {
            throw new System.NotImplementedException();
        }

        public Listing AddImage(ListingWithImage listing)
        {
            throw new System.NotImplementedException();
        }

        public Listing AddDigitalFile(DigitalListingWithImages digitalListingWithImages)
        {
            throw new System.NotImplementedException();
        }
    }
}