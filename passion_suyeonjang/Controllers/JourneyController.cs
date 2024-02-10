using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using passion_suyeonjang.Models;
using System.Web.Script.Serialization;
using static passion_suyeonjang.Models.Travelers;
using passion_suyeonjang.Models.ViewModels;

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

        // GET: Journey/Details/5
        public ActionResult Details(int id)
        {
            DetailsJourney ViewModel = new DetailsJourney();

            //objective: communicate with journey data api to retrieve one journey
            //curl https://localhost:44362/api/journeydata/findjourney/{id}

            string url = "journeydata/findjourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            JourneyDto SelectedJourney = response.Content.ReadAsAsync<JourneyDto>().Result;
            Debug.WriteLine("animal received : ");
            Debug.WriteLine(SelectedJourney.JourneyTitle);

            ViewModel.SelectedJourney = SelectedJourney;

            //show associated travelers with this journey
            url = "journeydata/listtravelersforjourney/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TravelerDto> ResponsibleTravelers = response.Content.ReadAsAsync<IEnumerable<TravelerDto>>().Result;

            ViewModel.ResponsibleTravelers = ResponsibleTravelers;

            url = "keeperdata/listkeepersnotcaringforanimal/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TravelerDto> AvailableTravelers = response.Content.ReadAsAsync<IEnumerable<TravelerDto>>().Result;

            ViewModel.AvailableTravelers = AvailableTravelers;


            return View(ViewModel);
        }


        //POST: Journey/Associate/{JourneyId}/{TravelerId}
        [HttpPost]
        public ActionResult Associate(int id, int TravelerId)
        {
            Debug.WriteLine("Attempting to associate journey :" + id + " with traveler " + TravelerId);

            //call our api to associate journey with traveler
            string url = "animaldata/associatejourneywithtraveler/" + id + "/" + TravelerId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // POST: Journey/Create
        [HttpPost]
        public ActionResult Create(Journeys journeys)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(journey.JourneyName);
            //objective: add a new journey into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44362/api/journeydata/addjourney
            string url = "journeydata/addjourney";


            string jsonpayload = jss.Serialize(journeys);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Journey/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateJourney ViewModel = new UpdateJourney();

            //the existing animal information
            string url = "journeydata/findjourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JourneyDto SelectedJourney = response.Content.ReadAsAsync<JourneyDto>().Result;
            ViewModel.SelectedJourney = SelectedJourney;

            // all traveler to choose from when updating this journey
            //the existing journey information
            url = "travelerdata/listtravelers/";
            response = client.GetAsync(url).Result;
            IEnumerable<TravelerDto> TravelersOptions = response.Content.ReadAsAsync<IEnumerable<TravelerDto>>().Result;

            ViewModel.TravelersOptions = TravelersOptions;

            return View(ViewModel);
        }

        // POST: Journey/Update/5
        [HttpPost]
        public ActionResult Update(int id, Journeys journeys)
        {

            string url = "journeydata/updatejourney/" + id;
            string jsonpayload = jss.Serialize(journeys);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Journey/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "journeydata/findjourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JourneyDto selectedanimal = response.Content.ReadAsAsync<JourneyDto>().Result;
            return View(selectedanimal);
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "journeydata/deletejourney/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}