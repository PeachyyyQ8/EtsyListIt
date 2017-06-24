using System;

namespace EtsyListIt.Utility.DomainObjects
{
    public class EtsyListItException : Exception
    {
        public EtsyListItException(string message) : base(message)
        {
            
        }
    }
}