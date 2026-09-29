using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    // GET: /api/home/stats
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats(
        CancellationToken cancellationToken)
    {
        var stats = await _homeService.GetStatsAsync(
            cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Home statistics retrieved successfully.",
            data = stats
        });
    }
}