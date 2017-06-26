using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using Machine.Fakes;
using Machine.Specifications;
using RestSharp;
using Rhino.Mocks;

namespace EtsyWrapper.Unittests
{
    public class AuthenticationWrapperTests : WithSubject<EtsyAuthenticationWrapper>
    {
        protected static TemporaryToken output;
        private Establish context = () =>
        {
            The<IRestServiceWrapper>()
            .WhenToldTo(x => x.GetRestClient())
            .Return(new RestClient());

            The<IRestServiceWrapper>()
            .WhenToldTo(x => x.GetRestRequest(Arg<string>.Is.Anything, Arg<Method>.Is.Anything))
            .Return(new RestRequest());
        };

        

        Because of = () => output = Subject.GetTemporaryCredentials("laksdf", "askdlfj",new  [] { "alskdjf" });

        private It response_should_parse_query_String = () => output.LoginURL.ShouldEqual("laksdjf");
    }
}
