using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Musiquizza_React.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Musiquizza_React.Controllers
{
    [Produces("application/json")]
    [EnableCors("AllowAllOrigins")]
    [Route("api/Lyrics")]
    public class LyricsController : Controller
    {
        public static Song SongReturned;
        private readonly SongService _songService;

        private readonly SpotifyService _spotifyService;

        public LyricsController(SongService songService, SpotifyService spotifyService)
        {
            _songService = songService;
            _spotifyService = spotifyService;
        }
        
        [HttpGet("GetLyric")]
        public async Task<JsonResult> Get()
        {

            //choose a random number and pull song on that id
            Random r = new Random();
            int rInt = r.Next(0, 64); //for ints

            SongReturned = await _songService.GetSong(rInt);
            var q = SongReturned.Title + " " + SongReturned.Artist;
            var access_token = await HttpContext.GetTokenAsync("Spotify", "access_token");
            SpotifyUser spotifyUser = _spotifyService.GetUserProfile(access_token);

            SpotifyTracks songs = _spotifyService.SearchTracks(spotifyUser.UserId, access_token, q);
            var uri = songs.Tracks.Items.Count() > 0 ? songs.Tracks.Items.FirstOrDefault().Uri : "";

            await SongReturned.GetLyrics();

            return Json(new {lyrics = SongReturned.SongLyric, uri = uri});
        }

        [HttpPost]
        public JsonResult Post([FromBody]QuizAnswer answer)
        {
            //if artist and title are the same as posted, return true
            if (answer.Artist.Equals(SongReturned.Artist, StringComparison.CurrentCultureIgnoreCase) && answer.Title.Equals(SongReturned.Title, StringComparison.CurrentCultureIgnoreCase))
            {
                return Json(new { isCorrect = true });
            }
            else
            {
                return Json(new { isCorrect = false });
            }

        }




    }
}
