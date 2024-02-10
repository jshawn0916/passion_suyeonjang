using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static passion_suyeonjang.Models.Travelers;

namespace passion_suyeonjang.Models.ViewModels
{
    public class DetailsJourney
    {
        public JourneyDto SelectedJourney { get; set; }
        public IEnumerable<TravelerDto> ResponsibleTravelers { get; set; }

        public IEnumerable<TravelerDto> AvailableTravelers { get; set; }
    }
}