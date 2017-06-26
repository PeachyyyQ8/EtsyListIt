namespace EtsyWrapper.DomainObjects
{
    public class PermanentToken
    {
        public string SharedSecret { get; set; }
        public string TokenID { get; set; }
        public string TokenSecret { get; set; }
        public string APIKey { get; set; }

        public bool IsValidEtsyToken()
        {
            return !string.IsNullOrEmpty(APIKey) &&
            !string.IsNullOrEmpty(SharedSecret) && 
            !string.IsNullOrEmpty(TokenID) &&
            !string.IsNullOrEmpty(TokenSecret);
        }
    }
}