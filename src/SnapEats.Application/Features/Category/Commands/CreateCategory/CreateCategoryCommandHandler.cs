using AutoMapper;
using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Category.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly CategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IRealTimeNotificationService _notificationService;

    public CreateCategoryCommandHandler(
        CategoryRepository categoryRepository,
        IMapper mapper,
        IRealTimeNotificationService notificationService)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var isNameUnique = await _categoryRepository.IsNameUniqueAsync(request.Name, cancellationToken);
        if (!isNameUnique)
            throw new InvalidCategoryException($"Category with name '{request.Name}' already exists.");

        var category = new SnapEats.Infrastructure.Persistence.Entities.Category
        {
            Name = request.Name,
            Description = request.Description
        };

        var result = await _categoryRepository.AddAsync(category, cancellationToken);

        await _notificationService.NotifyCategoryChangedAsync(new CategoryChangedEvent(
            Action: "Created",
            CategoryId: result.CategoryId,
            Name: result.Name,
            Description: result.Description
        ), cancellationToken);

        return result.CategoryId;
    }
}


