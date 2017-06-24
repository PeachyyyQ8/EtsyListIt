using EtsyWrapper;
using EtsyWrapper.Interfaces;
using StructureMap;

namespace EtsyListIt
{
    internal class DependencyRegistry : Registry
    {
        public DependencyRegistry()
        {
            For<IListingWrapper>().Use<ListingWrapper>();
        }
    }
}
