# Identity-Server-4-Web-API-Access
Use Identity Server to Control Web API Access


## [InMemory DB](https://stackoverflow.com/questions/48061096/why-cant-i-call-the-useinmemorydatabase-method-on-dbcontextoptionsbuilder/48062124)

According to EF Core: Testing with InMemory reference, you need to add the Microsoft.EntityFrameworkCore.InMemory package to use UseInMemoryDatabase() extension method with DbContextOptionsBuilder:

Install-Package Microsoft.EntityFrameworkCore.InMemory
Afterwards, you can follow example given in "Writing tests" section like this:

var options = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(databaseName: "database_name").Options;

using (var context = new ProductContext(options))
{
    // add service here
}

## [Use EF InMemory DB Example](https://exceptionnotfound.net/ef-core-inmemory-asp-net-core-store-database
/)



## Test using Postman 

Before putting [Authorize] attribute on our CustomerController, we can do CRUS by using:

GET http://localhost:57602/api/Customers - get all customers
GET http://localhost:57602/api/Customers/3 - get a specific customers
POST http://localhost:57602/api/Customers - Create a customers
Body, raw with JSON (application/json):
{
    "Id": 1,
    "FirstName": "John",
    "LastName": "Doe"
}
PUT http://localhost:57602/api/Customers/ - Update a customers
Body, raw with JSON (application/json):
{
    "Id": 1,
    "FirstName": "John",
    "LastName": "Smith"
}

DELETE http://localhost:57602/api/Customers/ - Delete a customers

Then, the [Authorize] attribute make all above getting Status: "401 Unauthorized"

## Getting IdentityServer4's OpenId-Configuration

GET http://localhost:5000/.well-known/openid-configuration
{
    "issuer": "http://localhost:5000",
    "jwks_uri": "http://localhost:5000/.well-known/openid-configuration/jwks",
    "authorization_endpoint": "http://localhost:5000/connect/authorize",
    "token_endpoint": "http://localhost:5000/connect/token",
    "userinfo_endpoint": "http://localhost:5000/connect/userinfo",
    "end_session_endpoint": "http://localhost:5000/connect/endsession",
    "check_session_iframe": "http://localhost:5000/connect/checksession",
    "revocation_endpoint": "http://localhost:5000/connect/revocation",
    "introspection_endpoint": "http://localhost:5000/connect/introspect",
    "device_authorization_endpoint": "http://localhost:5000/connect/deviceauthorization",
    "frontchannel_logout_supported": true,
    "frontchannel_logout_session_supported": true,
    "backchannel_logout_supported": true,
    "backchannel_logout_session_supported": true,
    "scopes_supported": [
        "WebAPI",
        "offline_access"
    ],
    "claims_supported": [],
    "grant_types_supported": [
        "authorization_code",
        "client_credentials",
        "refresh_token",
        "implicit",
        "urn:ietf:params:oauth:grant-type:device_code"
    ],
    "response_types_supported": [
        "code",
        "token",
        "id_token",
        "id_token token",
        "code id_token",
        "code token",
        "code id_token token"
    ],
    "response_modes_supported": [
        "form_post",
        "query",
        "fragment"
    ],
    "token_endpoint_auth_methods_supported": [
        "client_secret_basic",
        "client_secret_post"
    ],
    "id_token_signing_alg_values_supported": [
        "RS256"
    ],
    "subject_types_supported": [
        "public"
    ],
    "code_challenge_methods_supported": [
        "plain",
        "S256"
    ],
    "request_parameter_supported": true
}

## Getting Token using Postman

POST http://localhost:5000/connect/token
Authorization:

	Type: Basic Auth
	Username: client
	Password: secret

Body: 

    x-www-form-urlencoded
    |key|value|
    |--| --|
    |grant_type|client_credentials|
    |scope|WebAPI|

Header:

|key|value|
|---|---|
|Content-Type|application/x-www-urlencoded|
|Authorization|Basic Y2xpZW50OnNlY3JldA==|  


Response:
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImRjdVBMYng5OGtyR2tzNElEclI1NHciLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1ODM1Mzc0NTAsImV4cCI6MTU4MzU0MTA1MCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiV2ViQVBJIiwiY2xpZW50X2lkIjoiY2xpZW50Iiwic2NvcGUiOlsiV2ViQVBJIl19.nxhgXCwfUOhhLzbS9Ks2EuDJCy_tmTHLufgKKZYTec4DVar3DpxApZtSjwhIqNwRrrB8VHQDiuRz2C3fshZglItZphVgrPluihV28y5kMCqM3p1AAJ20IDh6Hofs-hPSrFuI8qypdv5myab1OgLnlauP61BC5yiK62fkbs0VOGW8u81VnmK8fgVNsdcr6vJaVcPcU7xvWGyGTbwhofol-n06p6eGG12vpA5vJE6MfwTlhAxGIlfonunlZyxrd5DXqjQtqYWvj86OjX2uSmN2jxeSJNOPCeIiM4ccLqP4IjB6rr41ZD4As7VNraos9vyesDPw_-3l50uW-GmFvLLELQ",
    "expires_in": 3600,
    "token_type": "Bearer",
    "scope": "WebAPI"
}

- Note, look at the Config.cs to see how we get the value for the above.
```csharp

    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource(**"WebAPI"**, "Customer API for Web API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client", // unique Id of the client
                    AllowedGrantTypes = **GrantTypes.ClientCredentials,**
                    ClientSecrets = { new Secret(**"secret"**.Sha256()) }, // add to the collection
                    AllowedScopes = {**"WebAPI"**} // add to the collection to indicate which WebAPI is allowed
                }
            };
        }
    }
``` 
