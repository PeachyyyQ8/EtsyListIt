using EtsyListIt.Utility;
using EtsyListIt.Utility.Interfaces;
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
            For<ISettingsUtility>().Use<SettingsUtility>();
            For<ICommandLineUtility>().Use<CommandLineUtility>();
            For<IEtsyAuthenticationWrapper>().Use<EtsyAuthenticationWrapper>();
            For<IRestServiceWrapper>().Use<RestServiceWrapper>();
        }
    }
}
