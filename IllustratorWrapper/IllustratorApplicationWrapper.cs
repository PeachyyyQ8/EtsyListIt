using Illustrator;
using IllustratorWrapper.DomainObjects;
using IllustratorWrapper.Interfaces;

namespace IllustratorWrapper
{
    public class IllustratorApplicationWrapper : IIllustratorApplicationWrapper

    {
        public IllustratorDocument Open(object file, AiDocumentColorSpace documentgColorSpace, object options = null)
        {
            var IllustratorDocument = new IllustratorDocument();
        }
    }
}