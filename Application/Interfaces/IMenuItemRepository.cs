using Domain.Entities;

namespace Application.Interfaces;

public interface IMenuItemRepository
{
    Task<(IReadOnlyList<MenuItem> Items, int TotalCount)>
        GetByRestaurantAsync(
            int restaurantId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
}