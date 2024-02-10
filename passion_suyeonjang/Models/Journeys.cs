using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace passion_suyeonjang.Models
{
    public class Journeys
    {
        //what discribes a journeys?
        //journey title, explain, destination
        [Key]
        public int JourneyId { get; set; }
        public string JourneyTitle {  get; set; }
        public string JourneyExplain { get; set; }

        //a journey has a traveler ID
        //a traveler has many journeys

        [ForeignKey("Travelers")]
        public int TravelerId { get; set; }
        public virtual Travelers Travelers { get; set; }
    }

    public class JourneyDto { 
        public int JourneyId { get; set; }
        public string JourneyTitle { get;  set; }
    }
}