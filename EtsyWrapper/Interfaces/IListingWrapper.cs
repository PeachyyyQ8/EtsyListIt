using EtsyWrapper.DomainObjects;

namespace EtsyWrapper.Interfaces
{
    public interface IListingWrapper
    {
        Listing CreateListing(Listing listing);

        Listing CreateListingWithImage(ListingWithImage listing);

        Listing CreateDigitalListingWithImage(DigitalListingWithImage digitalListingWithImage);

        Listing AddImage(ListingWithImage listing);

        Listing AddDigitalFile(DigitalListingWithImage digitalListingWithImage);
    }
}