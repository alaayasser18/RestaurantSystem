using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class HomeService : IHomeService
{
    private readonly ICityRepository _cityRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public HomeService(
        ICityRepository cityRepository,
        IRestaurantRepository restaurantRepository,
        IMenuItemRepository menuItemRepository)
    {
        _cityRepository = cityRepository;
        _restaurantRepository = restaurantRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IReadOnlyList<CityDto>> GetCitiesAsync(
        CancellationToken cancellationToken = default)
    {
        var cities = await _cityRepository
            .GetAllAsync(cancellationToken);

        return cities
            .Select(city => new CityDto
            {
                Id = city.Id,
                Name = city.Name
            })
            .ToList();
    }

    public async Task<(IReadOnlyList<RestaurantDto> Items, int TotalCount)>
        GetRestaurantsAsync(
            string? search,
            int? cityId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
    {
        var result = await _restaurantRepository.GetAllAsync(
            search,
            cityId,
            page,
            pageSize,
            cancellationToken);

        var items = result.Items
            .Select(restaurant => new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                City = new CityDto
                {
                    Id = restaurant.City.Id,
                    Name = restaurant.City.Name
                },
                Category = restaurant.Category,
                Rating = restaurant.Rating,
                Likes = restaurant.Likes,
                Comments = restaurant.Comments,
                Description = restaurant.Description,
                ImageUrl = restaurant.ImageUrl
            })
            .ToList();

        return (items, result.TotalCount);
    }

    public async Task<RestaurantDetailsDto?>
        GetRestaurantByIdAsync(
            int restaurantId,
            CancellationToken cancellationToken = default)
    {
        var result = await _restaurantRepository
            .GetAllAsync(
                null,
                null,
                1,
                int.MaxValue,
                cancellationToken);

        var restaurant = result.Items
            .FirstOrDefault(x => x.Id == restaurantId);

        if (restaurant is null)
        {
            return null;
        }

        var menuItemCount = await GetMenuCountAsync(
            restaurantId,
            cancellationToken);

        return new RestaurantDetailsDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            City = new CityDto
            {
                Id = restaurant.City.Id,
                Name = restaurant.City.Name
            },
            Category = restaurant.Category,
            Description = restaurant.Description,
            ImageUrl = restaurant.ImageUrl,
            Rating = restaurant.Rating,
            Likes = restaurant.Likes,
            Comments = restaurant.Comments,
            MenuItemCount = menuItemCount
        };
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

    public Task<HomeStatsDto> GetStatsAsync(
        CancellationToken cancellationToken = default)
    {
        var stats = new HomeStatsDto
        {
            RestaurantCount = 137,
            PeopleServed = 158,
            HappyService = "659K",
            RegularUsers = 235
        };

        return Task.FromResult(stats);
    }

    private async Task<int> GetMenuCountAsync(
        int restaurantId,
        CancellationToken cancellationToken)
    {
        var result = await _menuItemRepository
            .GetByRestaurantAsync(
                restaurantId,
                1,
                100,
                cancellationToken);

        return result.TotalCount;
    }
}