using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.Category.DTOs;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.Category.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDetailDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryDetailDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Include(c => c.MenuItems.Where(m => m.IsAvailable == true))
            .FirstOrDefaultAsync(c => c.CategoryId == request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Category with Id '{request.Id}' not found.");

        return _mapper.Map<CategoryDetailDto>(category);
    }
}

