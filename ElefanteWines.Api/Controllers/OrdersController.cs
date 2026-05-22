using ElefanteWines.Application.Interfaces;
using ElefanteWines.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ElefanteWines.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IWineRepository _wineRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrdersController(
        IOrderRepository orderRepository,
        IWineRepository wineRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _wineRepository = wineRepository;
        _customerRepository = customerRepository;
    }

    // GET /api/orders
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var orders = await _orderRepository.GetAllAsync(ct);
        return Ok(orders);
    }

    // GET /api/orders/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, ct);
        if (order is null) return NotFound(new { message = $"Orden con id {id} no encontrada." });
        return Ok(order);
    }

    // GET /api/orders/by-customer/{customerId}
    [HttpGet("by-customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken ct)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId, ct);
        return Ok(orders);
    }

    // POST /api/orders
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        // Validamos que exista el customer
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, ct);
        if (customer is null) return BadRequest(new { message = "El cliente no existe." });

        try
        {
            var order = new Order(dto.CustomerId);

            // Por cada item, buscamos el vino, lo agregamos a la orden (esto descuenta stock).
            foreach (var item in dto.Items)
            {
                var wine = await _wineRepository.GetByIdAsync(item.WineId, ct);
                if (wine is null)
                    return BadRequest(new { message = $"Vino con id {item.WineId} no existe." });

                order.AddItem(wine, item.Quantity);

                // Guardamos el cambio de stock del vino
                await _wineRepository.UpdateAsync(wine, ct);
            }

            await _orderRepository.AddAsync(order, ct);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Por ejemplo: stock insuficiente
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record CreateOrderDto(Guid CustomerId, List<OrderItemDto> Items);
public record OrderItemDto(Guid WineId, int Quantity);