using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    // ============================================================
    // CREATE ORDER
    // ============================================================

    public async Task<OrderDto> CreateOrderAsync(
        CreateOrderDto request,
        CancellationToken cancellationToken = default)
    {
        // --------------------------------------------------------
        // 1. Validate restaurant
        // --------------------------------------------------------

        var restaurantExists =
            await _orderRepository.RestaurantExistsAsync(
                request.RestaurantId,
                cancellationToken);

        if (!restaurantExists)
        {
            throw new KeyNotFoundException(
                "Restaurant not found.");
        }

        // --------------------------------------------------------
        // 2. Validate items
        // --------------------------------------------------------

        if (request.Items is null ||
            request.Items.Count == 0)
        {
            throw new ArgumentException(
                "Order must contain at least one item.");
        }

        // --------------------------------------------------------
        // 3. Get restaurant + requested menu items
        // --------------------------------------------------------

        var menuItemIds = request.Items
            .Select(item => item.MenuItemId)
            .Distinct()
            .ToList();

        var restaurant =
            await _orderRepository
                .GetRestaurantWithMenuItemsAsync(
                    request.RestaurantId,
                    menuItemIds,
                    cancellationToken);

        if (restaurant is null)
        {
            throw new KeyNotFoundException(
                "Restaurant not found.");
        }

        // --------------------------------------------------------
        // 4. Validate all menu items
        // --------------------------------------------------------

        var requestedItems =
            request.Items
                .GroupBy(item => item.MenuItemId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(item => item.Quantity));

        foreach (var requestedItem in requestedItems)
        {
            if (requestedItem.Value <= 0)
            {
                throw new ArgumentException(
                    "Item quantity must be greater than 0.");
            }
        }

        var availableMenuItems =
            restaurant.MenuItems
                .Where(item => item.IsAvailable)
                .ToDictionary(
                    item => item.Id);

        foreach (var requestedItem in requestedItems)
        {
            if (!availableMenuItems.ContainsKey(
                    requestedItem.Key))
            {
                var menuItemExists =
                    restaurant.MenuItems.Any(
                        item => item.Id == requestedItem.Key);

                if (menuItemExists)
                {
                    throw new InvalidOperationException(
                        $"Menu item with ID {requestedItem.Key} is currently unavailable.");
                }

                throw new KeyNotFoundException(
                    $"Menu item with ID {requestedItem.Key} was not found in this restaurant.");
            }
        }

        // --------------------------------------------------------
        // 5. Build order items using database prices
        // --------------------------------------------------------

        var orderItems =
            new List<OrderItem>();

        foreach (var requestedItem in requestedItems)
        {
            var menuItem =
                availableMenuItems[requestedItem.Key];

            var quantity =
                requestedItem.Value;

            var lineTotal =
                menuItem.Price * quantity;

            orderItems.Add(
                new OrderItem
                {
                    MenuItemId = menuItem.Id,

                    Name = menuItem.Name,

                    ImageUrl = menuItem.ImageUrl,

                    UnitPrice = menuItem.Price,

                    Quantity = quantity,

                    LineTotal = lineTotal
                });
        }

        // --------------------------------------------------------
        // 6. Calculate totals on the server
        // --------------------------------------------------------

        var subtotal =
            orderItems.Sum(
                item => item.LineTotal);

        // Currently delivery is free.
        // We can change this later if needed.
        var deliveryFee = 0m;

        var total =
            subtotal + deliveryFee;

        // --------------------------------------------------------
        // 7. Create order
        // --------------------------------------------------------

        var order = new Order
        {
            RestaurantId = restaurant.Id,

            RestaurantName = restaurant.Name,

            RestaurantImageUrl = restaurant.ImageUrl,

            CustomerName =
                request.CustomerName.Trim(),

            CustomerPhone =
                request.CustomerPhone.Trim(),

            CustomerEmail =
                request.CustomerEmail.Trim(),

            CustomerAddress =
                request.CustomerAddress.Trim(),

            Subtotal = subtotal,

            DeliveryFee = deliveryFee,

            Total = total,

            CreatedAt = DateTime.UtcNow,

            Items = orderItems
        };

        // --------------------------------------------------------
        // 8. Save order
        // --------------------------------------------------------

        var savedOrder =
            await _orderRepository.AddAsync(
                order,
                cancellationToken);

        // --------------------------------------------------------
        // 9. Return response
        // --------------------------------------------------------

        return MapToDto(savedOrder);
    }

    // ============================================================
    // GET ORDER
    // ============================================================

    public async Task<OrderDto?> GetOrderByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var order =
            await _orderRepository.GetByIdAsync(
                orderId,
                cancellationToken);

        if (order is null)
        {
            return null;
        }

        return MapToDto(order);
    }

    // ============================================================
    // MAP ENTITY → DTO
    // ============================================================

    private static OrderDto MapToDto(
        Order order)
    {
        return new OrderDto
        {
            Id = order.Id,

            RestaurantId =
                order.RestaurantId,

            RestaurantName =
                order.RestaurantName,

            RestaurantImageUrl =
                order.RestaurantImageUrl,

            CustomerName =
                order.CustomerName,

            CustomerPhone =
                order.CustomerPhone,

            CustomerEmail =
                order.CustomerEmail,

            CustomerAddress =
                order.CustomerAddress,

            Subtotal =
                order.Subtotal,

            DeliveryFee =
                order.DeliveryFee,

            Total =
                order.Total,

            CreatedAt =
                order.CreatedAt,

            Items =
                order.Items
                    .Select(item => new OrderItemDto
                    {
                        Id = item.Id,

                        MenuItemId =
                            item.MenuItemId,

                        Name =
                            item.Name,

                        ImageUrl =
                            item.ImageUrl,

                        UnitPrice =
                            item.UnitPrice,

                        Quantity =
                            item.Quantity,

                        LineTotal =
                            item.LineTotal
                    })
                    .ToList()
        };
    }
}