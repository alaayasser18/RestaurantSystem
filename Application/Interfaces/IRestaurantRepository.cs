using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces;

public interface IRestaurantRepository
{
    Task<(IReadOnlyList<Restaurant> Items, int TotalCount)> GetAllAsync(
        string? search,
        int? cityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
