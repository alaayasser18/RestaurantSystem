using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly AppDbContext _context;

    public RestaurantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Restaurant> Items, int TotalCount)> GetAllAsync(
        string? search,
        int? cityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Restaurant> query = _context.Restaurants
            .AsNoTracking()
            .Include(x => x.City);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();

            query = query.Where(x =>
                x.Name.Contains(normalizedSearch));
        }

        if (cityId.HasValue)
        {
            query = query.Where(x =>
                x.CityId == cityId.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}