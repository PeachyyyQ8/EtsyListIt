using EtsyWrapper.DomainObjects;

namespace EtsyWrapper.Interfaces
{
    public interface IListingWrapper
    {
        Listing CreateListing(Listing listing);

        Listing CreateListingWithImage(ListingWithImage listing);

        DigitalListingWithImages CreateDigitalListingWithImage(DigitalListingWithImages digitalListingWithImages);

        Listing AddImage(ListingWithImage listing);

        Listing AddDigitalFile(DigitalListingWithImages digitalListingWithImages);
    }
}