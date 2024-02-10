using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace passion_suyeonjang.Models
{
    public class Travelers
    {
        //what discribes a traveler?
        //first name, last name, email
        [Key]
        public int TravelerId { get; set; }

        public string TravelerFirstName { get; set; }
        public string TravelerLastName { get; set; }
        public string TravelerEmail { get; set; }

        //a traveler has many journeys
        public ICollection<Journeys> Journeys { get; set; }

        public class TravelerDto
        {
            public int TravelerId { get; set; }
            public string TravelerFirstName { get; set; }
            public string TravelerLastName { get; set; }
            public string TravelerEmail { get; set; }


        }
    }
}