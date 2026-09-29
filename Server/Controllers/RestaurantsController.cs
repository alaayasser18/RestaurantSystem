using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController : ControllerBase
{
    private const int DefaultPageSize = 6;
    private const int MaxPageSize = 12;

    private readonly IHomeService _homeService;

    public RestaurantsController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    // ============================================================
    // GET ALL RESTAURANTS
    // ============================================================

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRestaurants(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        page = NormalizePage(page);
        pageSize = NormalizePageSize(pageSize);

        var result = await _homeService.GetRestaurantsAsync(
            null,
            null,
            page,
            pageSize,
            cancellationToken);

        return Ok(CreatePagedResponse(
            "Restaurants retrieved successfully.",
            result.Items,
            result.TotalCount,
            page,
            pageSize));
    }

    // ============================================================
    // GET RESTAURANT DETAILS
    // ============================================================

    [HttpGet("{restaurantId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurantById(
        [FromRoute] int restaurantId,
        CancellationToken cancellationToken = default)
    {
        if (restaurantId <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Restaurant ID must be greater than 0.",
                data = (object?)null
            });
        }

        var restaurant = await _homeService
            .GetRestaurantByIdAsync(
                restaurantId,
                cancellationToken);

        if (restaurant is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Restaurant not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            success = true,
            message = "Restaurant retrieved successfully.",
            data = restaurant
        });
    }

    // ============================================================
    // GET RESTAURANT MENU
    // ============================================================

    [HttpGet("{restaurantId:int}/menu")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurantMenu(
        [FromRoute] int restaurantId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        if (restaurantId <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Restaurant ID must be greater than 0.",
                data = (object?)null
            });
        }

        if (page <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Page must be greater than 0.",
                data = (object?)null
            });
        }

        if (pageSize <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Page size must be greater than 0.",
                data = (object?)null
            });
        }

        if (pageSize > MaxPageSize)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    $"Page size cannot be greater than {MaxPageSize}.",
                data = (object?)null
            });
        }

        var restaurant = await _homeService
            .GetRestaurantByIdAsync(
                restaurantId,
                cancellationToken);

        if (restaurant is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Restaurant not found.",
                data = (object?)null
            });
        }

        var result = await _homeService.GetMenuItemsAsync(
            restaurantId,
            page,
            pageSize,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(
            result.TotalCount / (double)pageSize);

        if (result.TotalCount > 0 && page > totalPages)
        {
            return NotFound(new
            {
                success = false,
                message =
                    $"Menu page {page} does not exist.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            success = true,
            message = "Restaurant menu retrieved successfully.",
            data = new
            {
                restaurant,
                items = result.Items,
                page,
                pageSize,
                totalCount = result.TotalCount,
                totalPages,
                hasNextPage = page < totalPages,
                hasPreviousPage = page > 1
            }
        });
    }

    // ============================================================
    // SEARCH
    // ============================================================

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchRestaurants(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return BadRequest(new
            {
                success = false,
                message = "Search value is required.",
                data = Array.Empty<RestaurantDto>()
            });
        }

        page = NormalizePage(page);
        pageSize = NormalizePageSize(pageSize);

        var result = await _homeService.GetRestaurantsAsync(
            search,
            null,
            page,
            pageSize,
            cancellationToken);

        return Ok(CreatePagedResponse(
            result.TotalCount == 0
                ? $"No restaurants found for '{search}'."
                : "Restaurants found successfully.",
            result.Items,
            result.TotalCount,
            page,
            pageSize));
    }

    // ============================================================
    // CITY
    // ============================================================

    [HttpGet("city")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRestaurantsByCity(
        [FromQuery] int? cityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        if (!cityId.HasValue || cityId.Value <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "City ID is required and must be greater than 0.",
                data = Array.Empty<RestaurantDto>()
            });
        }

        page = NormalizePage(page);
        pageSize = NormalizePageSize(pageSize);

        var result = await _homeService.GetRestaurantsAsync(
            null,
            cityId.Value,
            page,
            pageSize,
            cancellationToken);

        return Ok(CreatePagedResponse(
            result.TotalCount == 0
                ? $"No restaurants found in city with ID {cityId.Value}."
                : "Restaurants retrieved successfully.",
            result.Items,
            result.TotalCount,
            page,
            pageSize));
    }

    // ============================================================
    // SEARCH + CITY
    // ============================================================

    [HttpGet("search-and-city")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchRestaurantsByNameAndCity(
        [FromQuery] string? search,
        [FromQuery] int? cityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return BadRequest(new
            {
                success = false,
                message = "Search value is required.",
                data = Array.Empty<RestaurantDto>()
            });
        }

        if (!cityId.HasValue || cityId.Value <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "City ID is required and must be greater than 0.",
                data = Array.Empty<RestaurantDto>()
            });
        }

        page = NormalizePage(page);
        pageSize = NormalizePageSize(pageSize);

        var result = await _homeService.GetRestaurantsAsync(
            search,
            cityId.Value,
            page,
            pageSize,
            cancellationToken);

        return Ok(CreatePagedResponse(
            result.TotalCount == 0
                ? "No restaurants found for the selected filters."
                : "Restaurants found successfully.",
            result.Items,
            result.TotalCount,
            page,
            pageSize));
    }

    // ============================================================
    // HELPERS
    // ============================================================

    private static int NormalizePage(int page)
    {
        return page < 1 ? 1 : page;
    }

    private static int NormalizePageSize(int pageSize)
    {
        if (pageSize < 1)
        {
            return DefaultPageSize;
        }

        return Math.Min(pageSize, MaxPageSize);
    }

    private static object CreatePagedResponse(
        string message,
        IReadOnlyList<RestaurantDto> items,
        int totalCount,
        int page,
        int pageSize)
    {
        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new
        {
            success = true,
            message,
            data = new
            {
                items,
                page,
                pageSize,
                totalCount,
                totalPages,
                hasNextPage = page < totalPages,
                hasPreviousPage = page > 1
            }
        };
    }
}