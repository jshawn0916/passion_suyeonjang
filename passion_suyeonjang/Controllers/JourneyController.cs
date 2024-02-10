using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using passion_suyeonjang.Models;
using System.Web.Script.Serialization;

namespace passion_suyeonjang.Controllers
{
    public class JourneyController : Controller
    {
        
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JourneyController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44362/api/");
        }

        // GET: Journey/List
        // Objective :  a webpage that lists journeys in our system
        public ActionResult List()
        {
            //use HTTP client to access information

            HttpClient client = new HttpClient();
            //set the url
            string url = "journeydata/listjourneys";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<JourneyDto> Journeys = response.Content.ReadAsAsync<IEnumerable<JourneyDto>>().Result;
            //Debug.WriteLine(Journeys);

            return View(Journeys);
        }
    }
}