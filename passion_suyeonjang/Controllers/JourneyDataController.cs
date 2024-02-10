using passion_suyeonjang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using static passion_suyeonjang.Models.Travelers;

namespace passion_suyeonjang.Controllers
{
    public class JourneyDataController : ApiController
    {
        //utlizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all journeys in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all journeys in the database, including their associated travelers.
        /// </returns>
        /// <example>
        /// GET: api/JourneyData/ListJourneys
        /// </example>
        [HttpGet]
        [ResponseType(typeof(JourneyDto))]
        public IHttpActionResult ListJourneys()
        {
            //sending a query to the database
            //select * from journeys...

            List<Journeys> Journeys = db.Journeys.ToList();
            List<JourneyDto> JourneyDtos = new List<JourneyDto>();

            Journeys.ForEach(j => JourneyDtos.Add(new JourneyDto() 
            { 
                JourneyId = j.JourneyId,
                JourneyTitle = j.JourneyTitle,
            }
            ));

            //read through the results...
            //push the results to the list of journeys to return
            return Ok(JourneyDtos);
        }

        /// <summary>
        /// Associates a particular traveler with a particular journey
        /// </summary>
        /// <param name="journeyid">The journey ID primary key</param>
        /// <param name="travelerid">The traveler ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/JourneyData/AssociateJourneyWithTraveler/9/1
        /// </example>
        [HttpPost]
        [Route("api/JourneyData/AssociateJourneyWithTraveler/{journeyid}/{travelerid}")]
        public IHttpActionResult AssociateJourneyWithTraveler(int journeyid, int travelerid)
        {

            Journeys SelectedJourney = db.Journeys.Include(j => j.Travelers).Where(j => j.JourneyId == journeyid).FirstOrDefault();
            Travelers SelectedTraveler = db.Travelers.Find(travelerid);

            if (SelectedJourney == null || SelectedTraveler == null)
            {

                return NotFound();
            }

            Debug.WriteLine("input journey id is: " + journeyid);
            Debug.WriteLine("selected journey title is: " + SelectedJourney.JourneyTitle);
            Debug.WriteLine("input traveler id is: " + travelerid);
            Debug.WriteLine("selected traveler first name is: " + SelectedTraveler.TravelerFirstName);
            Debug.WriteLine("selected traveler last name is: " + SelectedTraveler.TravelerLastName);

            //SQL equivalent:
            //insert into travelerjourneys (journeyid, travelerid) values ({jid},{tid})
           
            SelectedJourney.Travelers.Add(SelectedTraveler);
            db.SaveChanges();

            return Ok();
        }

        //UpdateJourney
        /// <summary>
        /// Updates a particular journey in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Journey ID primary key</param>
        /// <param name="journey">JSON FORM DATA of an journey</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/UpdateJourney/5
        /// FORM DATA: Journey JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateJourney(int id, Journeys journeys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != journeys.JourneyId)
            {

                return BadRequest();
            }

            db.Entry(journeys).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JourneyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JourneyExists(int id)
        {
            return db.Journeys.Count(e => e.JourneyId == id) > 0;
        }

        //AddJourney
        /// <summary>
        /// Adds a journey to the system
        /// </summary>
        /// <param name="journey">JSON FORM DATA of an journey</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Journey ID, Journey Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/AddJourney
        /// FORM DATA: Journey JSON Object
        /// </example>
        [ResponseType(typeof(Journeys))]
        [HttpPost]
        public IHttpActionResult AddJourney(Journeys journeys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Journeys.Add(journeys);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = journeys.JourneyId }, journeys);
        }

        //DeleteJourney
        /// <summary>
        /// Deletes a journey from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the journey</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/DeleteJourney/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Journeys))]
        [HttpPost]
        public IHttpActionResult DeleteJourney(int id)
        {
            Journeys journeys = db.Journeys.Find(id);
            if (journeys == null)
            {
                return NotFound();
            }

            db.Journeys.Remove(journeys);
            db.SaveChanges();

            return Ok();
        }




        //related methods include:

        //ListJourneysForTraveler
      
        [HttpGet]
        [ResponseType(typeof(JourneyDto))]
        public IHttpActionResult ListJourneyForTraveler(int id)
        {
            //SQL equivalent:
            //select journeys.*, travelerjourneys.* from journeys INNER JOIN 
            //travelerjourneys on journeys.journeyid = travelerjourneys.journeyid
            //where travelerjourneys.travelerid={TRAVELERID}

            //all journeys that have travelers which match with our ID
            List<Journeys> journeys = db.Journeys.Where(
                j => j.Travelers.Any(
                    t => t.TravelerId == id
                )).ToList();
            List<JourneyDto> JourneyDtos = new List<JourneyDto>();

            

            foreach (Journeys journey in journeys)
            {
                JourneyDtos.Add(new JourneyDto()
                {
                    JourneyId = journey.JourneyId,
                    JourneyTitle = journey.JourneyTitle,
                });
            }

            return Ok(JourneyDtos);
        }
        //ListJourneysForDestination
        //AddJourneyToDestination
        //RemoveJourneyFromDestination 

    }
}
