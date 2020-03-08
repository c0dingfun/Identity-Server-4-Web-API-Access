using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello World!");
        //}

        const string IdendityServer4Address = "http://localhost:5000";
        const string WebApiEndPoint= "http://localhost:5001/api/customers";

        // clever way to do async Main()
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover Identity Server 4's endpoints via its discovery document
            var client = new HttpClient(); // Note: Both DiscoveryClient and TokenClient were deprecated
            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(IdendityServer4Address);

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return; // without "discovery document" we can not go on
            }

            // obtain the access token, for this client
            TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "client",
                        ClientSecret = "secret",
                        Scope = "WebAPI"
                    }
                );

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return; // without "access token" we can not go on
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // Ready to consume the WebAPI 
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(   // construct the payload for adding new customer
                JsonConvert.SerializeObject(
                        new
                        {
                            Id = 12,
                            FirstName = "Kenny",
                            LastName = "Doe"
                        }),
                        Encoding.UTF8,
                        "application/json");

            // post to the WebAPI to add new customer
            HttpResponseMessage createCustomerResponse = await apiClient.PostAsync(WebApiEndPoint, customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }
            else
            {
                Console.WriteLine("Added new Custeromer");
            }

            // from the WebAI, get a list of the customers 
            HttpResponseMessage getCustomerReponse = await apiClient.GetAsync(WebApiEndPoint);
            if (!getCustomerReponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerReponse.StatusCode);
            }
            else
            {
                string content = await getCustomerReponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadLine();
        }
    }

}
