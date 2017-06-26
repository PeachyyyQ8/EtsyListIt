using EtsyListIt.Utility.DomainObjects;

namespace EtsyListIt.Utility.Interfaces
{
    public interface ICommandLineUtility
    {
        EtsyListItArgs ParseCommandLineArguments(string[] args);
    }
}