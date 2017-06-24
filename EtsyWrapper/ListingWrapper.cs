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

        public Listing CreateDigitalListingWithImage(DigitalListingWithImage digitalListingWithImage)
        {
            throw new System.NotImplementedException();
        }

        public Listing AddImage(ListingWithImage listing)
        {
            throw new System.NotImplementedException();
        }

        public Listing AddDigitalFile(DigitalListingWithImage digitalListingWithImage)
        {
            throw new System.NotImplementedException();
        }
    }
}