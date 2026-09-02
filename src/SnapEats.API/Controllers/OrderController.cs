using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapEats.Application.Features.CustomerOrder.Commands.CancelOrder;
using SnapEats.Application.Features.CustomerOrder.Commands.CreateOrder;
using SnapEats.Application.Features.CustomerOrder.Commands.DeleteOrder;
using SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrder;
using SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrderStatus;
using SnapEats.Application.Features.CustomerOrder.Queries.GetAllOrders;
using SnapEats.Application.Features.CustomerOrder.Queries.GetOrderById;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly OrderInvoiceRepository _orderInvoiceRepository;

    public OrderController(IMediator mediator, OrderInvoiceRepository orderInvoiceRepository)
    {
        _mediator = mediator;
        _orderInvoiceRepository = orderInvoiceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("{orderId:int}/invoice")]
    public async Task<IActionResult> GetInvoice(int orderId, CancellationToken cancellationToken)
    {
        var invoice = await _orderInvoiceRepository.GetInvoiceAsync(orderId, cancellationToken);
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("Id mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _mediator.Send(new CancelOrderCommand { OrderId = id });
        return NoContent();
    }

    [HttpPut("{id:int}/update")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteOrderCommand { Id = id });
        return NoContent();
    }
}
