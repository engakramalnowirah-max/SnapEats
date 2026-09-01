using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.Category.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IRealTimeNotificationService _notificationService;

    public DeleteCategoryCommandHandler(
        ApplicationDbContext context,
        IRealTimeNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(c => c.MenuItems)
            .FirstOrDefaultAsync(c => c.CategoryId == request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Category with Id '{request.Id}' not found.");

        var categoryName = category.Name;

        if (category.MenuItems != null && category.MenuItems.Any())
        {
            var menuItemIds = category.MenuItems.Select(m => m.MenuItemId).ToList();
            var orderItems = await _context.OrderItems
                .Where(oi => menuItemIds.Contains(oi.MenuItemId))
                .ToListAsync(cancellationToken);

            if (orderItems.Any())
            {
                _context.OrderItems.RemoveRange(orderItems);
            }

            _context.MenuItems.RemoveRange(category.MenuItems);
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.NotifyCategoryChangedAsync(new CategoryChangedEvent(
            Action: "Deleted",
            CategoryId: request.Id,
            Name: categoryName
        ), cancellationToken);
    }
}

