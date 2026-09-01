using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.MenuItem.Commands.UpdateMenuItem;

public sealed class UpdateMenuItemCommandHandler : IRequestHandler<UpdateMenuItemCommand>
{
    private readonly MenuItemRepository _menuItemRepository;
    private readonly IRealTimeNotificationService _notificationService;

    public UpdateMenuItemCommandHandler(
        MenuItemRepository menuItemRepository,
        IRealTimeNotificationService notificationService)
    {
        _menuItemRepository = menuItemRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Menu item with Id '{request.Id}' not found.");

        menuItem.Name = request.Name;
        menuItem.Description = request.Description;
        menuItem.Price = request.Price;
        menuItem.ImageUrl = request.ImageUrl ?? string.Empty;
        menuItem.IsAvailable = request.IsAvailable;

        await _menuItemRepository.UpdateAsync(menuItem, cancellationToken);

        await _notificationService.NotifyMenuItemChangedAsync(new MenuItemChangedEvent(
            Action: "Updated",
            MenuItemId: menuItem.MenuItemId,
            Name: menuItem.Name,
            Price: menuItem.Price ?? 0,
            CategoryId: menuItem.CategoryId,
            IsAvailable: menuItem.IsAvailable ?? true
        ), cancellationToken);

    }
}


