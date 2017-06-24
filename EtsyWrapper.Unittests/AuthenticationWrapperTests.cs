using EtsyWrapper.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;

namespace EtsyWrapper.Unittests
{
    public class AuthenticationWrapperTests : WithSubject<AuthenticationWrapper>
    {
        protected static TemporaryToken output;
        private Establish context = () =>
        {

        };

        Because of = () => output = Subject.GetTemporaryCredentials("laksdf", "askdlfj",new  [] { "alskdjf" });

        private It response_should_parse_query_String = () => output.ConfirmURL.ShouldEqual("laksdjf");
    }
}
