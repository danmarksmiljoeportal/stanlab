using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Dmp.Examples.Stanlab
{
    public partial class StanlabClient
    {
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            if (_accessToken == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (_expiration <= DateTime.UtcNow)
            {
                RequestAccessToken().Wait();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        private DateTime _expiration;
        private string _accessToken;

        private Func<Task> RequestAccessToken;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">Token endpoint address</param>
        /// <param name="clientId">System integration client identifier</param>
        /// <param name="clientSecret">System integration client password</param>
        /// <param name="scope"></param>
        /// <returns></returns>
        ///
        public async Task AddAuthorization(string authority, string clientId, string clientSecret, string scope = null)
        {
            var tokenEndpoint = await GetTokenEndpoint(authority);

            RequestAccessToken = async () =>
            {
                var client = new TokenClient(tokenEndpoint, clientId, clientSecret);

                var response = await client.RequestClientCredentialsAsync(scope);
  
                if (response.IsError)
                {
                    throw new UnauthorizedAccessException(response.Error);
                }

                _accessToken = response.AccessToken;
                _expiration = DateTime.UtcNow.AddSeconds(response.ExpiresIn - 60);
            };

            await RequestAccessToken();
        }

        private async Task<string> GetTokenEndpoint(string authority)
        {
            var client = new HttpClient();

            var discoveryDocument = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authority,
                Policy = new DiscoveryPolicy
                {
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            });

            if (discoveryDocument.IsError)
            {
                throw new AuthenticationException(discoveryDocument.Error);
            }

            return discoveryDocument.TokenEndpoint;
        }
    }
}