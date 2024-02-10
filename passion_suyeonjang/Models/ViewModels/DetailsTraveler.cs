using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static passion_suyeonjang.Models.Travelers;

namespace passion_suyeonjang.Models.ViewModels
{
    public class DetailsTraveler
    {
        public TravelerDto SelectedTraveler { get; set; }
        public IEnumerable<JourneyDto> KeptJourneys { get; set; }
    }
}