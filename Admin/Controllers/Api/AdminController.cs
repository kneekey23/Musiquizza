using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using AWSAppService.Data;
using AWSAppService.Auth;
using Microsoft.Extensions.Options;
using Admin;

namespace Musiquizza_React.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        private readonly AWSOptions _awsOptions;
        private readonly CloudAuthService _cloudAuthService;
        private readonly DBDataService _dbDataService;

        
        public AdminController(IOptions<AWSOptions> awsOptions)
        {
            _awsOptions = awsOptions.Value;
            Amazon.RegionEndpoint AppRegion = Amazon.RegionEndpoint.GetBySystemName(_awsOptions.Region);

            _cloudAuthService = new CloudAuthService(_awsOptions.CognitoPoolId, AppRegion);
            _dbDataService = new DBDataService(_cloudAuthService.GetAWSCredentials(), AppRegion);
        }
        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]QuizAnswer songToAdd)
        {
            int songID = await _dbDataService.GetItemCount<Song>();

            Song newSong = new Song();
            newSong.Artist = songToAdd.Artist;
            newSong.Title = songToAdd.Title;
            newSong.SongID = songID + 1;

            _dbDataService.PostToDB(newSong);
            
            return Json(new { added = true });
        }
        
        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
