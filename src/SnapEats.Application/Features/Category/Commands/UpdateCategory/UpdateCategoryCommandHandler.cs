using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Category.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly CategoryRepository _categoryRepository;
    private readonly IRealTimeNotificationService _notificationService;

    public UpdateCategoryCommandHandler(
        CategoryRepository categoryRepository,
        IRealTimeNotificationService notificationService)
    {
        _categoryRepository = categoryRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Category with Id '{request.Id}' not found.");

        var isNameUnique = await _categoryRepository.IsNameUniqueAsync(request.Name, cancellationToken);
        if (!isNameUnique && !string.Equals(category.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            throw new InvalidCategoryException($"Category with name '{request.Name}' already exists.");

        category.Name = request.Name;
        category.Description = request.Description;

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        await _notificationService.NotifyCategoryChangedAsync(new CategoryChangedEvent(
            Action: "Updated",
            CategoryId: category.CategoryId,
            Name: category.Name,
            Description: category.Description
        ), cancellationToken);
    }
}


