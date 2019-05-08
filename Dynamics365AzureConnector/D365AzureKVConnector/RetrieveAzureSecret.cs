using System;
using System. Collections. Generic;
using System. IO;
using System. Linq;
using System. Net. Http;
using System. Net. Http. Headers;
using System. Runtime. Serialization. Json;
using System. Text;
using System. Threading. Tasks;

namespace D365AzureKVConnector
{
    public class RetrieveAzureSecret
    {

       

        public string GetSecretByName (Api api, string vaultName , string secretName)
        {            

            //Retrieve the latest version of a secret by name
            var getKeyByNameTask = Task.Run(async () => await GetSecretByName(api.Token, vaultName, secretName));
            Task. WaitAll ( getKeyByNameTask );
            if ( getKeyByNameTask. Result == null )
                throw new Exception ( "Error retrieving secret versions from key vault" );
            var retrievedSecretUrl = getKeyByNameTask.Result;

            // Retrieve a secret by its url
            var getKeyByUrlTask2 = Task.Run(async () => await GetSecretByUrl(api.Token, retrievedSecretUrl));
            Task. WaitAll ( getKeyByUrlTask2 );
            if ( getKeyByUrlTask2. Result == null )
                throw new Exception ( "Error retrieving secret value from key vault" );
            //Deserialize the vault response to get the secret
            GetSecretResponse getSecretResponse2 = Helper.DeserializeObject<GetSecretResponse>(getKeyByUrlTask2.Result);
            //returnedValue is the Azure Key Vault Secret
            string returnedValue2 = getSecretResponse2.value;

            return returnedValue2;
        }

       
        //Get the Secret value from the Key Vault by url - api-version is required
        private async Task<string> GetSecretByUrl ( string token , string secretUrl )
        {
            using ( HttpClient httpClient = new HttpClient ( ) )
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                        new Uri(secretUrl + "?api-version=2016-10-01"));
                request. Headers. Authorization = new AuthenticationHeaderValue ( "Bearer" , token );

                HttpResponseMessage response = await httpClient.SendAsync(request);

                return !response. IsSuccessStatusCode ? null
                    : response. Content. ReadAsStringAsync ( ). Result;
            }
        }

        //Get the most recent, enabled Secret value by name  - api-version is required
        private async Task<string> GetSecretByName ( string token , string vaultName , string secretName )
        {
            string nextLink = vaultName + "/secrets/" + secretName + "/versions?api-version=2016-10-01";
            List<Value> values = new List<Value>();

            using ( HttpClient httpClient = new HttpClient ( ) )
            {
                while ( !string. IsNullOrEmpty ( nextLink ) )
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                        new Uri(nextLink));
                    request. Headers. Authorization = new AuthenticationHeaderValue ( "Bearer" , token );

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if ( !response. IsSuccessStatusCode )
                        return null;

                    var versions = Helper.DeserializeObject<GetSecretVersionsResponse>(response.Content.ReadAsStringAsync().Result);
                    values. AddRange ( versions. value );
                    nextLink = versions. nextLink;
                }
            }

            Value mostRecentValue =
                values.Where(a => a.attributes.enabled)
                    .OrderByDescending(a => UnixTimeToUtc(a.attributes.created))
                    .FirstOrDefault();

            return mostRecentValue?.id;
        }

       

       
        private DateTime UnixTimeToUtc ( double unixTime )
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = TimeSpan.FromSeconds(unixTime);
            return epoch. Add ( timeSpan ). ToUniversalTime ( );
        }
    }
}
