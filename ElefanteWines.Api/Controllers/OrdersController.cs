using ElefanteWines.Application.Features.Orders.Commands.CreateOrder;
using ElefanteWines.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IMediator mediator, IOrderRepository orderRepository)
    {
        _mediator = mediator;
        _orderRepository = orderRepository;
    }

    // GET /api/orders — lectura simple
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var orders = await _orderRepository.GetAllAsync(ct);
        return Ok(orders);
    }

    // GET /api/orders/{id} — lectura con relaciones
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, ct);
        if (order is null)
            return NotFound(new { message = $"Orden con id {id} no encontrada." });
        return Ok(order);
    }

    // GET /api/orders/by-customer/{customerId}
    [HttpGet("by-customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken ct)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId, ct);
        return Ok(orders);
    }

    // POST /api/orders — usa Command (caso de uso complejo)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        try
        {
            var orderId = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = orderId }, new { id = orderId });
        }
        catch (InvalidOperationException ex)
        {
            // Por ejemplo: stock insuficiente, cliente no existe, vino no existe
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}