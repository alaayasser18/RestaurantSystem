using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // Restaurant snapshot
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(150)]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string RestaurantImageUrl { get; set; } = string.Empty;

        // Customer information
        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string CustomerAddress { get; set; } = string.Empty;

        // Prices
        public decimal Subtotal { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; }

        // Order items
        public ICollection<OrderItem> Items { get; set; } =
            new List<OrderItem>();
    }
}