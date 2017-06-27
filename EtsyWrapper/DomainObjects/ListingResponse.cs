using System.Runtime.Serialization;

namespace EtsyWrapper.DomainObjects
{
    [DataContract]
    public class ListingResponse : Listing
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "results")]
        public Listing[] Listing { get; set; }
    }
}