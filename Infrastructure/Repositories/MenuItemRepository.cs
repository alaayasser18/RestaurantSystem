using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<MenuItem> Items, int TotalCount)>
        GetByRestaurantAsync(
            int restaurantId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
    {
        var query = _context.MenuItems
            .AsNoTracking()
            .Where(x =>
                x.RestaurantId == restaurantId &&
                x.IsAvailable);

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}