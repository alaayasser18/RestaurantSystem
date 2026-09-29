using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RestaurantQueryDto
    {
        [StringLength(
            100,
            ErrorMessage = "Restaurant search cannot exceed 100 characters.")]
        public string? Search { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "City ID must be greater than 0.")]
        public int? CityId { get; set; }
    }
}
