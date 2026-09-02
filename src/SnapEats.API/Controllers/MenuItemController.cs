using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapEats.Application.Features.MenuItem.Commands.CreateMenuItem;
using SnapEats.Application.Features.MenuItem.Commands.DeleteMenuItem;
using SnapEats.Application.Features.MenuItem.Commands.UpdateMenuItem;
using SnapEats.Application.Features.MenuItem.Queries.GetAllMenuItems;
using SnapEats.Application.Features.MenuItem.Queries.GetMenuItemById;
using SnapEats.Application.Features.MenuItem.Queries.SearchMenuItems;

namespace SnapEats.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class MenuItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMenuItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetMenuItemByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchMenuItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMenuItemCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteMenuItemCommand { Id = id });
        return NoContent();
    }
}
