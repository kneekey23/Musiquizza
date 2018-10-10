using Newtonsoft.Json;
using System;

namespace Musiquizza_React.Models {
    public class SpotifyUser
    {
        [JsonProperty("id")]
        public string UserId { get; set; }
        [JsonProperty("display_name")]
        public String DisplayName { get; set; }
    }
}