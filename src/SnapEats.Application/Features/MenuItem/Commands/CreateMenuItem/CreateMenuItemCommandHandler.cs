using AutoMapper;
using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.MenuItem.Commands.CreateMenuItem;

public sealed class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, int>
{
    private readonly MenuItemRepository _menuItemRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IRealTimeNotificationService _notificationService;

    public CreateMenuItemCommandHandler(
        MenuItemRepository menuItemRepository,
        CategoryRepository categoryRepository,
        IMapper mapper,
        IRealTimeNotificationService notificationService)
    {
        _menuItemRepository = menuItemRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new InvalidCategoryException($"Category with Id '{request.CategoryId}' not found.");

        var menuItem = new SnapEats.Infrastructure.Persistence.Entities.MenuItem
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl ?? string.Empty,
            IsAvailable = request.IsAvailable
        };

        var result = await _menuItemRepository.AddAsync(menuItem, cancellationToken);

        await _notificationService.NotifyMenuItemChangedAsync(new MenuItemChangedEvent(
            Action: "Created",
            MenuItemId: result.MenuItemId,
            Name: result.Name,
            Price: result.Price ?? 0,
            CategoryId: result.CategoryId,
            IsAvailable: result.IsAvailable ?? true
        ), cancellationToken);


        return result.MenuItemId;
    }
}


