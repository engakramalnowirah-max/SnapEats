using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.MenuItem.Commands.DeleteMenuItem;

public sealed class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IRealTimeNotificationService _notificationService;

    public DeleteMenuItemCommandHandler(
        ApplicationDbContext context,
        IRealTimeNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = await _context.MenuItems
            .FirstOrDefaultAsync(m => m.MenuItemId == request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Menu item with Id '{request.Id}' not found.");

        var menuItemName = menuItem.Name;
        var categoryId = menuItem.CategoryId;
        var price = menuItem.Price ?? 0;


        var orderItems = await _context.OrderItems
            .Where(oi => oi.MenuItemId == request.Id)
            .ToListAsync(cancellationToken);

        if (orderItems.Any())
        {
            _context.OrderItems.RemoveRange(orderItems);
        }

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.NotifyMenuItemChangedAsync(new MenuItemChangedEvent(
            Action: "Deleted",
            MenuItemId: request.Id,
            Name: menuItemName,
            Price: price,
            CategoryId: categoryId,
            IsAvailable: false
        ), cancellationToken);
    }
}

