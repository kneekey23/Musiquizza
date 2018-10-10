using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
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

        public SpotifyTracks SearchTracks(string userId, string access_token, string q)
        {
            string url = String.Format("https://api.spotify.com/v1/search?q={0}&type=track&include_external=audio", q);
            SpotifyTracks tracks = GetFromSpotify<SpotifyTracks>(url, access_token);

            return tracks;
        }
    }
}