using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServer
{
    /// <summary>
    /// Code as Configuration
    /// </summary>
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource(name:"WebAPI", displayName: "Customer API for Web API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client", // unique Id of the client (like login)
                    ClientSecrets = { new Secret("secret".Sha256()) }, // like password
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"WebAPI"} // add to the collection to indicate which WebAPI is allowed
                }
            };
        }
    }
}
