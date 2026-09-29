using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CityId { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Rating { get; set; }

        public int Likes { get; set; }

        public int Comments { get; set; }

        public City City { get; set; } = null!;

        public ICollection<MenuItem> MenuItems { get; set; } =
            new List<MenuItem>();
    }
}
