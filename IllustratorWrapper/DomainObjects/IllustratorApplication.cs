using System;
using Illustrator;

namespace IllustratorWrapper.DomainObjects
{
    public class IllustratorApplication
    {
        private dynamic _application;
        public IllustratorApplication()
        {
            Type type = Type.GetTypeFromProgID("Illustrator.Application");
            _application = Activator.CreateInstance(type);
        }
    }
}