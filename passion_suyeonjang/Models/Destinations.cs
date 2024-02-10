using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace passion_suyeonjang.Models
{
    public class Destinations
    {
        //what discribes a destination?
        //name, category, location
        [Key]
        public int DestinationId { get; set; }

        public string DestinationName { get; set; }

        public string DestinationCategory { get; set; }

        public string DestinationLocation {  get; set; }

        //public string DestinationDescription { get; set; }
    }
}