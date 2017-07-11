using System;
using System.Net;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace EtsyWrapper
{
    public class ListingWrapper : IListingWrapper
    {
        private readonly IRestServiceWrapper _restServiceWrapper;
        private readonly RestClient _restClient;

        public ListingWrapper(IRestServiceWrapper restServiceWrapper)
        {
            _restServiceWrapper = restServiceWrapper;
            _restClient = _restServiceWrapper.GetRestClient();
        }

        public Listing CreateListing(Listing listing, PermanentToken authToken)
        {
            listing.State = "draft";
            if (!authToken.IsValidEtsyToken())
            {
                throw new EtsyWrapperException("Auth token is not valid!  Please authenticate before calling the CreateListing method.");
            }
            RestRequest request = _restServiceWrapper.GetRestRequest("listings", Method.POST);

            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForProtectedResource(authToken);

            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(listing), ParameterType.RequestBody);

            var etsyResponse = _restClient.Execute(request);
            if (etsyResponse.StatusCode != HttpStatusCode.Created)
            {
                throw new EtsyWrapperException(
                    $"Create Listing failed.  Please check your parameters and try again. Error: {etsyResponse.Content}");
            }
            var listingResponse = JsonConvert.DeserializeObject<ListingResponse>(etsyResponse.Content);
            listing.ID = listingResponse.Listing[0].ID;
            return listing;
        }
        
        public Listing CreateListingWithImage(Listing listing, PermanentToken authToken)
        {
            listing = CreateListing(listing, authToken);
            AddImageToListing(listing, authToken);
            return listing;
        }

        public void UpdateListing(Listing listing, PermanentToken authToken)
        {
            RestRequest request = _restServiceWrapper.GetRestRequest($"listings/{listing.ID}", Method.PUT);

            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForProtectedResource(authToken);

            request.AddHeader("Accept", "application/json");
            var etsyResponse = _restClient.Execute(request);
            if (etsyResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(
                    $"Create Listing failed.  Please check your parameters and try again. Error: {etsyResponse.Content}");
            }
        }

        public Listing CreateDigitalListingWithImages(Listing listing, PermanentToken authToken)
        {
            listing = CreateListingWithImage(listing, authToken);
            AddDigitalFileToListing(listing, authToken);
            return listing;
        }

        public bool AddImageToListing(Listing listing, PermanentToken token)
        {
            if (string.IsNullOrEmpty(listing.ID))
            {
                throw new EtsyWrapperException("AddImageToListing:  Listing ID can not be empty!");
            }
            RestRequest request = _restServiceWrapper.GetRestRequest($"listings/{listing.ID}/images", Method.POST);

            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForProtectedResource(token);

            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            foreach (var image in listing.Images)
            {
                if (image == null)
                {
                    continue;
                }
                request.AddFile("image", image.ImagePath);
                request.AddParameter("application/json", JsonConvert.SerializeObject(listing.Images), ParameterType.RequestBody);
                var etsyResponse = _restClient.Execute(request);
                if (etsyResponse.StatusCode != HttpStatusCode.Created)
                {
                    throw new Exception(
                        $"Create Listing Image failed.  Please check your parameters and try again. Error: {etsyResponse.Content}");
                }
            }

            return true;
        }

        public bool AddDigitalFileToListing(Listing listing, PermanentToken authToken)
        {
            if (string.IsNullOrEmpty(listing.ID))
            {
                throw new EtsyWrapperException("AddDigitalFileToListing:  Listing ID can not be empty!");
            }

            RestRequest request = _restServiceWrapper.GetRestRequest($"listings/{listing.ID}/files", Method.POST);

            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForProtectedResource(authToken);

            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            foreach (var file in listing.DigitalFiles)
            {
                if (file == null)
                {
                    continue;
                }
                request.AddFile("file", file.Path);
                request.AddParameter("name", file.Name);
                request.AddParameter("rank", file.Rank);
                //request.AddParameter("application/json", JsonConvert.SerializeObject(file), ParameterType.RequestBody);
                var etsyResponse = _restClient.Execute(request);
                if (etsyResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(
                        $"Create Listing File failed.  Please check your parameters and try again. Error: {etsyResponse.Content}");
                }
            }
            return true;
        }
    }
}