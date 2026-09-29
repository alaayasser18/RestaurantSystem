using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateOrderItemDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Menu item ID must be greater than 0.")]
        public int MenuItemId { get; set; }

        [Range(
            1,
            100,
            ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }
}