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
using passion_suyeonjang.Migrations;
using static passion_suyeonjang.Models.Travelers;

namespace passion_suyeonjang.Controllers
{
    public class TravelerDataController : ApiController
    {
        //utlizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all Travelers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Travelers in the database, including their associated journeys.
        /// </returns>
        /// <example>
        /// GET: api/TravelerData/ListTravelers
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TravelerDto))]
        public IHttpActionResult ListTravelers()
        {
            List<Travelers> Travelers = db.Travelers.ToList();
            List<TravelerDto> TravelerDtos = new List<TravelerDto>();

            Travelers.ForEach(t => TravelerDtos.Add(new TravelerDto()
            {
                TravelerId = t.TravelerId,
                TravelerFirstName = t.TravelerFirstName,
                TravelerLastName = t.TravelerLastName
            }));

            return Ok(TravelerDtos);
        }

        /// <summary>
        /// Returns all Travelers in the system associated with a particular journey.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Travelers in the database taking care of a particular journey
        /// </returns>
        /// <param name="id">Journey Primary Key</param>
        /// <example>
        /// GET: api/TravelerData/ListTravelersForJourney/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TravelerDto))]
        public IHttpActionResult ListTravelersForJourney(int id)
        {

            //SQL Equivalent:
            //select travelers.*, travelerjourneys.* from journeys inner join travelerjourneys on travelerjourneys.travelerid = traveler.travelerid where
            //travelerjourneys.journeyid = {id}

            List<Travelers> Travelers = db.Travelers.Where(
                t => t.Journeys.Any(
                    j => j.JourneyId == id)
                ).ToList();
            List<TravelerDto> TravelerDtos = new List<TravelerDto>();

            Travelers.ForEach(t => TravelerDtos.Add(new TravelerDto()
            {
                TravelerId = t.TravelerId,
                TravelerFirstName = t.TravelerFirstName,
                TravelerLastName = t.TravelerLastName
            }));

            return Ok(TravelerDtos);
        }

        /// <summary>
        /// Returns all Travelers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Traveler in the system matching up to the Traveler ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Traveler</param>
        /// <example>
        /// GET: api/TravelerData/FindTraveler/5
        /// </example>
        [ResponseType(typeof(TravelerDto))]
        [HttpGet]
        public IHttpActionResult FindTraveler(int id)
        {
            Travelers travelers = db.Travelers.Find(id);
            TravelerDto TravelerDto = new TravelerDto()
            {
                TravelerId = travelers.TravelerId,
                TravelerFirstName = travelers.TravelerFirstName,
                TravelerLastName = travelers.TravelerLastName
            };
            if (travelers == null)
            {
                return NotFound();
            }

            return Ok(TravelerDto);
        }
        /// <summary>
        /// Updates a particular Traveler in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Traveler ID primary key</param>
        /// <param name="Traveler">JSON FORM DATA of an Traveler</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/UpdateTraveler/5
        /// FORM DATA: Traveler JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTraveler(int id, Travelers Traveler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Traveler.TravelerId)
            {

                return BadRequest();
            }

            db.Entry(Traveler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelerExists(id))
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

        /// <summary>
        /// Adds an Traveler to the system
        /// </summary>
        /// <param name="travelers">JSON FORM DATA of an Traveler</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Traveler ID, Traveler Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/AddTraveler
        /// FORM DATA: Traveler JSON Object
        /// </example>
        [ResponseType(typeof(Travelers))]
        [HttpPost]
        public IHttpActionResult AddTraveler(Travelers travelers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Travelers.Add(travelers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = travelers.TravelerId }, travelers);
        }

        /// <summary>
        /// Deletes an Traveler from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Traveler</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/DeleteKeeper/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Travelers))]
        [HttpPost]
        public IHttpActionResult DeleteKeeper(int id)
        {
            Travelers travelers= db.Travelers.Find(id);
            if (travelers == null)
            {
                return NotFound();
            }

            db.Travelers.Remove(travelers);
            db.SaveChanges();

            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TravelerExists(int id)
        {
            return db.Travelers.Count(e => e.TravelerId == id) > 0;
        }
    }
}
