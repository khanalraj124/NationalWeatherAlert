using NationalWeatherAlert.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NationalWeatherAlert.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostingEnvironment)
        {
            _logger = logger;
            _webHostingEnvironment = webHostingEnvironment;
        }

        public IActionResult Index()
        {
            var rootPath = _webHostingEnvironment.WebRootPath; //get the root path

            var fullPathRegions = Path.Combine(rootPath, "json\\regions.json");
            var fullPathStates = Path.Combine(rootPath, "json\\states.json");

            var jsonRegions = System.IO.File.ReadAllText(fullPathRegions);
            var jsonStates = System.IO.File.ReadAllText(fullPathStates);

            List<Regions> lstRegions = new List<Regions>();
            List<States> lstStates = new List<States>();
            List<States> lstTmpStates = new List<States>();
            List<RegionwithAbb> lstRegionwithAbb = new List<RegionwithAbb>();
          
            if (!string.IsNullOrWhiteSpace(jsonRegions))
            {

                lstRegions = JsonConvert.DeserializeObject<List<Regions>>(jsonRegions); 
            }
            if (!string.IsNullOrWhiteSpace(jsonStates))
            {

                lstStates = JsonConvert.DeserializeObject<List<States>>(jsonStates);
            }

            foreach (var region in lstRegions)
            {
                RegionwithAbb regionwithAbb = new RegionwithAbb();
                lstTmpStates = lstStates;
                var filteredStates = lstTmpStates.FindAll(x => x.RegionId == region.Id);
                string abbreviation = string.Empty;
                foreach (var state in filteredStates)
                {
                    abbreviation += state.Abbreviation + ",";
                }
                regionwithAbb.Name= region.Name + "(" + abbreviation.TrimEnd(',') + ")";
                regionwithAbb.RegionId = region.Id;
                lstRegionwithAbb.Add(regionwithAbb);

            }
            ViewBag.RegionList = lstRegionwithAbb.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.Name.ToString(),
                                      Value = x.RegionId.ToString()
                                  });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Weather(int regionId)
        {
            
            var rootPath = _webHostingEnvironment.WebRootPath; //get the root path
            var fullPathStates = Path.Combine(rootPath, "json\\states.json");
            var jsonStates = System.IO.File.ReadAllText(fullPathStates);
            List<States> lstStates = new List<States>();
            List<States> filteredStates = new List<States>();
            if (!string.IsNullOrWhiteSpace(jsonStates))
            {
                lstStates = JsonConvert.DeserializeObject<List<States>>(jsonStates);
                filteredStates= lstStates.Where(x => x.RegionId == regionId).ToList();
            }
            ViewBag.states = filteredStates.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.Name.ToString(),
                                      Value = x.Abbreviation.ToString()
                                  });


            return View();
        }
        [HttpPost, ActionName("GetzoneByState")]
        public JsonResult GetzoneByState(string stateAbbr)
        {
            var rootPath = _webHostingEnvironment.WebRootPath; //get the root path
            var fullPathCities_and_zones = Path.Combine(rootPath, "json\\cities_and_zones.json");
            var jsonCities_and_zones = System.IO.File.ReadAllText(fullPathCities_and_zones);

            List<Zone_City> city_Zones = new List<Zone_City>();
            List<Zone_City> filtered_city_Zones = new List<Zone_City>();
            if (!string.IsNullOrEmpty(stateAbbr))
            {
                if (!string.IsNullOrWhiteSpace(jsonCities_and_zones))
                {
                    city_Zones = JsonConvert.DeserializeObject<List<Zone_City>>(jsonCities_and_zones);
                    filtered_city_Zones= city_Zones.Where(x => x.StateAbbreviation == stateAbbr).ToList();
                }
            }
            return Json(filtered_city_Zones);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}