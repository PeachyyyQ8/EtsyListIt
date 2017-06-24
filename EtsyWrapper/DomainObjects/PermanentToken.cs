namespace EtsyWrapper.DomainObjects
{
    public class PermanentToken
    {
        private string ApiKey { get; set; }
        public string SharedSecret { get; set; }
        public string TokenID { get; set; }
        public string TokenSecret { get; set; }
    }
}