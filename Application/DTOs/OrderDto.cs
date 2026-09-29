namespace Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public string RestaurantImageUrl { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string CustomerAddress { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } =
            new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }

        public int MenuItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }
    }
}