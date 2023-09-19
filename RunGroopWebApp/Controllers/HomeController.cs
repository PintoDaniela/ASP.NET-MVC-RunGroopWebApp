using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Helpers;
using System.Diagnostics;
using RunGroopWebApp.ViewModels;
using System.Net;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using System.Globalization;

namespace RunGroopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository)
        {
            _logger = logger;
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult>Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();

            try
            {
                string url = "https://ipinfo.io?token=8f285015d183b6";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;
                if(homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubsByCity(homeViewModel.City);
                }
                else
                {
                    homeViewModel.Clubs = null;
                }
                return View(homeViewModel);

            }
            catch (Exception ex)
            {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}