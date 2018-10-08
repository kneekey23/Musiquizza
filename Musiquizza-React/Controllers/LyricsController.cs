using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Musiquizza_React.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using Microsoft.AspNetCore.Cors;

namespace Musiquizza_React.Controllers
{
    [Produces("application/json")]
    [EnableCors("AllowAllOrigins")]
    [Route("api/Lyrics")]
    public class LyricsController : Controller
    {
        public static Song SongReturned;
        private readonly SongService _songService;

        private readonly SpotifySearchService _spotifyService;

        public LyricsController(SongService songService, SpotifySearchService spotifySearchService)
        {
            _songService = songService;
            _spotifyService = spotifySearchService;
        }

        [HttpGet("GetLyric")]
        public async Task<JsonResult> Get()
        {
            //if no token, get out because the rest will fail
            if(_spotifyService.token == null){
                return Json(new {lyrics = "", uri = ""});
            }

            //choose a random number and pull song on that id
            Random r = new Random();
            int rInt = r.Next(0, 64); //for ints

            SongReturned = await _songService.GetSong(rInt);
            var q = SongReturned.Title + " " + SongReturned.Artist;
            string uri = await _spotifyService.GetSongUri(q);

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

        [HttpGet("AuthSpotify")]
        public void AuthSpotify(){
            _spotifyService.AuthorizeSpotify();
        }


    }
}
