using Illustrator;
using IllustratorWrapper.DomainObjects;

namespace IllustratorWrapper.Interfaces
{
    public interface IIllustratorApplicationWrapper
    {
        IllustratorDocument Open(object file, AiDocumentColorSpace documentgColorSpace, object options = null);
    }
}