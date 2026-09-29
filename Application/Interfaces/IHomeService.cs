using Application.DTOs;

namespace Application.Interfaces;

public interface IHomeService
{
    Task<IReadOnlyList<CityDto>> GetCitiesAsync(
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<RestaurantDto> Items, int TotalCount)>
        GetRestaurantsAsync(
            string? search,
            int? cityId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

    Task<RestaurantDetailsDto?> GetRestaurantByIdAsync(
        int restaurantId,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<MenuItemDto> Items, int TotalCount)>
        GetMenuItemsAsync(
            int restaurantId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

    Task<HomeStatsDto> GetStatsAsync(
        CancellationToken cancellationToken = default);
}