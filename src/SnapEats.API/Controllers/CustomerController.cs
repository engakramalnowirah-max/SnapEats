using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapEats.Application.Features.Customer.Commands.DeleteCustomer;
using SnapEats.Application.Features.Customer.Commands.RegisterCustomer;
using SnapEats.Application.Features.Customer.Commands.UpdateCustomer;
using SnapEats.Application.Features.Customer.Queries.GetAllCustomers;
using SnapEats.Application.Features.Customer.Queries.GetCustomerById;

namespace SnapEats.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCustomersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCustomerCommand { Id = id });
        return NoContent();
    }
}

