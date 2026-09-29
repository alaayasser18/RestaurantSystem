using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // GET ORDER BY ID
    // ============================================================

    public async Task<Order?> GetByIdAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(
                order => order.Id == orderId,
                cancellationToken);
    }

    // ============================================================
    // ADD ORDER
    // ============================================================

    public async Task<Order> AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(
            order,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return order;
    }

    // ============================================================
    // CHECK RESTAURANT EXISTS
    // ============================================================

    public async Task<bool> RestaurantExistsAsync(
        int restaurantId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Restaurants
            .AnyAsync(
                restaurant => restaurant.Id == restaurantId,
                cancellationToken);
    }

    // ============================================================
    // GET RESTAURANT WITH SELECTED MENU ITEMS
    // ============================================================

    public async Task<Restaurant?> GetRestaurantWithMenuItemsAsync(
        int restaurantId,
        IEnumerable<int> menuItemIds,
        CancellationToken cancellationToken = default)
    {
        var ids = menuItemIds
            .Distinct()
            .ToList();

        return await _context.Restaurants
            .Include(restaurant => restaurant.MenuItems
                .Where(menuItem =>
                    ids.Contains(menuItem.Id)))
            .FirstOrDefaultAsync(
                restaurant => restaurant.Id == restaurantId,
                cancellationToken);
    }
}