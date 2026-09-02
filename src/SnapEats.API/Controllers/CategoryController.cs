using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnapEats.Application.Features.Category.Commands.CreateCategory;
using SnapEats.Application.Features.Category.Commands.DeleteCategory;
using SnapEats.Application.Features.Category.Commands.UpdateCategory;
using SnapEats.Application.Features.Category.Queries.GetAllCategories;
using SnapEats.Application.Features.Category.Queries.GetCategoryById;
using SnapEats.Application.Features.Category.Queries.SearchCategories;

namespace SnapEats.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchCategoriesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}
