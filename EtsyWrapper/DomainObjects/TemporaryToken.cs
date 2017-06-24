namespace EtsyWrapper.DomainObjects
{
    public class TemporaryToken
    {
        public string TokenID { get; set; }
        public string TokenSecret { get; set; }
        public string ConfirmURL { get; set; }
    }
}