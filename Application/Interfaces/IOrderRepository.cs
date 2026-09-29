using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<Order> AddAsync(
        Order order,
        CancellationToken cancellationToken = default);

    Task<bool> RestaurantExistsAsync(
        int restaurantId,
        CancellationToken cancellationToken = default);

    Task<Restaurant?> GetRestaurantWithMenuItemsAsync(
        int restaurantId,
        IEnumerable<int> menuItemIds,
        CancellationToken cancellationToken = default);
}