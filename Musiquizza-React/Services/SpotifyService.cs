using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using Musiquizza_React.Models;

namespace Musiquizza_React {
    public class SpotifyService
    {
        public SpotifyService()
        {
        }

        public SpotifyUser GetUserProfile(string token)
        {
            string url = "https://api.spotify.com/v1/me";
            SpotifyUser spotifyUser = GetFromSpotify<SpotifyUser>(url, token);
            return spotifyUser;
        }

        public T GetFromSpotify<T>(string url, string token)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Set("Authorization", "Bearer" + " " + token);
                request.ContentType = "application/json; charset=utf-8";

                T type = default(T);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            type = JsonConvert.DeserializeObject<T>(responseFromServer);
                        }
                    }
                }
                return type;
            }
            catch (WebException ex)
            {
                return default(T);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


         public string AuthorizeSpotify()
        {
            try
            {
                SpotifyAuthorization auth = new SpotifyAuthorization();
                WebRequest request = WebRequest.Create("https://accounts.spotify.com/api/token");
                request.Method = "POST";
                string clientId = "cd63690c687f48538e3f7e6b38ecd8f6";
                string clientSecret = "1643da7651574c358bb9414eb76be8e3";
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(clientId + ":" + clientSecret);
                var encoded64 = System.Convert.ToBase64String(plainTextBytes);
                request.Headers.Set("Authorization", "Basic" + " " + encoded64);
                request.ContentType = "application/x-www-form-urlencoded";

                var postData = "grant_type=client_credentials";
                var data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }


                using (WebResponse response = request.GetResponse())
                {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            auth = JsonConvert.DeserializeObject<SpotifyAuthorization>(responseFromServer);
                        }
                    
                }
                return auth.AccessToken;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public SpotifyTracks SearchTracks(string q)
        {
            string token = AuthorizeSpotify();
            string url = String.Format("https://api.spotify.com/v1/search?q={0}&type=track&include_external=audio", q);
            SpotifyTracks tracks = GetFromSpotify<SpotifyTracks>(url, token);

            return tracks;
        }
    }
}