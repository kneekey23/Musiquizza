using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using AWSAppService.Auth;
using AWSAppService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Admin.Controllers
{
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PostSong(Song songToAdd)
        {
            int songID = await _dbDataService.GetItemCount<Song>();

            songToAdd.SongID = songID + 1;

            _dbDataService.PostToDB(songToAdd);

            return Json(new { added = true });
        }

    }
}