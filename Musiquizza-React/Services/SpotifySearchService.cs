using System;
using System.IO;
using Amazon;
using System.Linq;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;

using Musiquizza_React.Models;


namespace Musiquizza_React {

    public class SpotifySearchService {
        public CredentialsAuth auth2;
        public Token token;
        public static SpotifyWebAPI spotify;
        public static string clientId = "";
        public static string clientSecret = "";
        public SpotifySearchService()
        {
           Init().Wait();
        }

        public async Task Init() {
            clientId = await GetSecret("SpotifyClientId");
            clientSecret = await GetSecret("SpotifyClientSecret");
           
             //Create the auth object
            // auth2 = new CredentialsAuth(clientId, clientSecret);
            // token = await auth2.GetToken();
            // spotify = new SpotifyWebAPI
            // {
            //     AccessToken = token.AccessToken,
            //     TokenType = token.TokenType
            // };
             
        } 

        public void AuthorizeSpotify() {
            AuthorizationCodeAuth auth = new AuthorizationCodeAuth(clientId, clientSecret, "https://localhost:5001", "https://localhost:5001",
                                Scope.Streaming | Scope.UserReadBirthdate | Scope.UserModifyPlaybackState | Scope.UserReadEmail | Scope.UserReadPrivate | Scope.UserReadPlaybackState | Scope.UserReadCurrentlyPlaying | Scope.UserReadRecentlyPlayed);
            auth.AuthReceived += AuthOnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
            auth.Stop(0);
        }

         private async void AuthOnAuthReceived(object sender, AuthorizationCode payload)
        {
            Console.WriteLine("WAHOO");
            AuthorizationCodeAuth auth = (AuthorizationCodeAuth) sender;
            auth.Stop();

            token = await auth.ExchangeCode(payload.Code);
            Console.WriteLine("Token" + token.AccessToken);
            spotify = new SpotifyWebAPI
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
      
        }



        public async Task<string> GetSongUri(string q){
            var tracks = await spotify.SearchItemsAsync(q, SearchType.Track, 10);
            string uri = tracks.Tracks.Items.Count > 0 ? tracks.Tracks.Items.FirstOrDefault().Uri : "";
            return uri;
        }

       
        public async Task<string> GetSecret(string secretName)
        {
            
            string region = "us-west-2";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = await client.GetSecretValueAsync(request);
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                throw;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (System.AggregateException ae)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
            JObject json = JObject.Parse(secret);
            
            return json[secretName].ToString();
        
        }


        
    }
}