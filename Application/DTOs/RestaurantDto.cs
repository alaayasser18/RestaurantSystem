using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RestaurantDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public CityDto City { get; set; } = new();

        public string Category { get; set; } = string.Empty;

        public decimal Rating { get; set; }

        public int Likes { get; set; }

        public int Comments { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
