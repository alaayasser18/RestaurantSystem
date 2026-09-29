using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly IHomeService _homeService;

    public CitiesController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    // GET: /api/cities
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities(
        CancellationToken cancellationToken)
    {
        var cities = await _homeService.GetCitiesAsync(
            cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Cities retrieved successfully.",
            data = cities
        });
    }
}