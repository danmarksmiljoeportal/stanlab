using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Dmp.Stanlab.References.StanlabClient
{
    public class TokenClient
    {
        public TokenClient(string authority)
        {
            _client = new HttpClient();
            Authority = authority ?? throw new ArgumentNullException(nameof(authority));            
        }

        private readonly HttpClient _client;

        private DiscoveryDocumentResponse _discoveryDocument;

        public string Authority { get; set; }

        public async Task<TokenResponse> RequestClientCredentialsToken(string clientId, string clientSecret, string scope)
        {
            if (_discoveryDocument == null)
            {
                _discoveryDocument = await GetDiscoveryDocument();
            }

            var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest 
            {
                Address = _discoveryDocument.TokenEndpoint,

                ClientId = clientId,
                ClientSecret = clientSecret,

                Scope = scope
            });

            if (response.IsError)
            {
                throw new UnauthorizedAccessException(response.Error);
            }

            return response;
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocument()
        {
            var discoveryDocument = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = Authority,
                Policy = new DiscoveryPolicy
                {
                    ValidateEndpoints = false
                }
            });

            if (discoveryDocument.IsError)
            {
                throw new AuthenticationException(discoveryDocument.Error);
            }

            return discoveryDocument;
        }
    }
}
