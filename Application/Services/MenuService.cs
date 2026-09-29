using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class MenuService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuService(
        IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<(IReadOnlyList<MenuItemDto> Items, int TotalCount)>
        GetMenuItemsAsync(
            int restaurantId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
    {
        var result = await _menuItemRepository
            .GetByRestaurantAsync(
                restaurantId,
                page,
                pageSize,
                cancellationToken);

        var items = result.Items
            .Select(item => new MenuItemDto
            {
                Id = item.Id,
                RestaurantId = item.RestaurantId,
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsAvailable = item.IsAvailable
            })
            .ToList();

        return (items, result.TotalCount);
    }
}