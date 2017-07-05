using EtsyWrapper.DomainObjects;

namespace EtsyWrapper.Interfaces
{
    public interface IListingWrapper
    {
        Listing CreateListing(Listing listing, PermanentToken authToken);
        Listing CreateListingWithImage(Listing listing, PermanentToken authToken);
        Listing CreateDigitalListingWithImages(Listing listing, PermanentToken authToken);
        bool AddImageToListing(Listing listing, PermanentToken authToken);
        bool AddDigitalFileToListing(Listing listing, PermanentToken authToken);
        void UpdateListing(Listing listing, PermanentToken authToken);
    }
}