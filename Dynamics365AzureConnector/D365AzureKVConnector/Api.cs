using System;
using System. Collections. Generic;
using System. Linq;
using System. Net. Http;
using System. Text;
using System. Threading. Tasks;

namespace D365AzureKVConnector
{
    public class Api
    {
        private Api ( ) { }
        private string _resourceUrl { get; set; }
        private static readonly string  _BASEAUTHURL = "https://login.windows.net/";
        private static readonly string  _SUFFIXAUTHURL = "/oauth2/token";
        public string Token { get { return _token; } }
        private string _token;
        public static Api GetAPI ( string clientId , string clientSecret , string tenantId , string resourceBaseUrl )
        {
            Api api = new Api ();
            //Retrieve the access token required for authentication
            var getTokenTask = Task.Run(async () => await GetToken(clientId, clientSecret, tenantId, resourceBaseUrl));
            Task. WaitAll ( getTokenTask );
            if ( getTokenTask. Result == null )
            { return null; }
            //Deserialize the token response to get the access token
            TokenResponse tokenResponse = Helper.DeserializeObject<TokenResponse>(getTokenTask.Result);
            api. _token = tokenResponse. access_token;

            return api;


        }

        //Get the access token required to access the Key Vault
        private static async Task<string> GetToken ( string clientId , string clientSecret , string tenantId , string resourceBaseUrl )
        {
            using ( HttpClient httpClient = new HttpClient ( ) )
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("resource", resourceBaseUrl),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                HttpResponseMessage response = await httpClient.PostAsync(
                    _BASEAUTHURL + tenantId + _SUFFIXAUTHURL, formContent);

                return !response. IsSuccessStatusCode ? null
                    : response. Content. ReadAsStringAsync ( ). Result;
            }
        }

    }
}
