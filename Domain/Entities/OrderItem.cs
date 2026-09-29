using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }

        public Order Order { get; set; } = null!;

        public MenuItem MenuItem { get; set; } = null!;
    }
}