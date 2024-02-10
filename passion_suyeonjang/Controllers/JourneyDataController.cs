using passion_suyeonjang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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
                JourneyTitle = j.JourneyTitle
            }
            ));

            //read through the results...
            //push the results to the list of journeys to return
            return Ok(JourneyDtos);
        }
        //FindJourney
        //AddJourney
        //UpdateJourney
        //DeleteJourney

        //related methods include:

        //ListJourneysForTraveler
        //ListJourneysForDestination
        //AddJourneyToDestination
        //RemoveJourneyFromDestination 

    }
}
