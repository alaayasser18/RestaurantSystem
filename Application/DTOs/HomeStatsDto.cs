using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class HomeStatsDto
    {
        public int RestaurantCount { get; set; }

        public int PeopleServed { get; set; }

        public string HappyService { get; set; } = string.Empty;

        public int RegularUsers { get; set; }
    }
}
