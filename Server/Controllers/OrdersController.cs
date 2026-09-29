using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    // ============================================================
    // CREATE ORDER
    // POST: /api/orders
    // ============================================================

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order =
                await _orderService.CreateOrderAsync(
                    request,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetOrderById),
                new
                {
                    orderId = order.Id
                },
                new
                {
                    success = true,
                    message = "Order created successfully.",
                    data = order
                });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message,
                data = (object?)null
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                success = false,
                message = ex.Message,
                data = (object?)null
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                data = (object?)null
            });
        }
    }

    // ============================================================
    // GET ORDER BY ID
    // GET: /api/orders/{orderId}
    // ============================================================

    [HttpGet("{orderId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(
        [FromRoute] int orderId,
        CancellationToken cancellationToken = default)
    {
        if (orderId <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Order ID must be greater than 0.",
                data = (object?)null
            });
        }

        var order =
            await _orderService.GetOrderByIdAsync(
                orderId,
                cancellationToken);

        if (order is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Order not found.",
                data = (object?)null
            });
        }

        return Ok(new
        {
            success = true,
            message = "Order retrieved successfully.",
            data = order
        });
    }
}