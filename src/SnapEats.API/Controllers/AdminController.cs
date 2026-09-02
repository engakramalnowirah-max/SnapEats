using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapEats.Application.Features.Admin.Commands.CreateAdmin;
using SnapEats.Application.Features.Admin.Commands.DeleteAdmin;
using SnapEats.Application.Features.Admin.Queries.GetAllAdmins;
using SnapEats.Application.Features.Admin.Queries.GetAdminById;

namespace SnapEats.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAdminsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetAdminByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public IActionResult GetDashboard()
    {
        return Ok(new { Message = "Admin dashboard", Timestamp = DateTime.UtcNow });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteAdminCommand { Id = id });
        return NoContent();
    }
}

