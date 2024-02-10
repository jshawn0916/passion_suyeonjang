using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static passion_suyeonjang.Models.Travelers;

namespace passion_suyeonjang.Models.ViewModels
{
    public class UpdateJourney
    {
        //This viewmodel is a class which stores information that we need to present to /Journey/Update/{}

        //the existing journey information

        public JourneyDto SelectedJourney { get; set; }

        // all species to choose from when updating this journey

        public IEnumerable<TravelerDto> TravelersOptions { get; set; }
    }
}