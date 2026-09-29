using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateOrderDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Restaurant ID must be greater than 0.")]
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(
            150,
            MinimumLength = 2,
            ErrorMessage = "Customer name must be between 2 and 150 characters.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^01[0125][0-9]{8}$",
            ErrorMessage = "Enter a valid Egyptian phone number.")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress(
            ErrorMessage = "Enter a valid email address.")]
        [StringLength(200)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(
            500,
            MinimumLength = 5,
            ErrorMessage = "Customer address must be between 5 and 500 characters.")]
        public string CustomerAddress { get; set; } = string.Empty;

        [Required]
        [MinLength(
            1,
            ErrorMessage = "Order must contain at least one item.")]
        public List<CreateOrderItemDto> Items { get; set; } =
            new();
    }
}