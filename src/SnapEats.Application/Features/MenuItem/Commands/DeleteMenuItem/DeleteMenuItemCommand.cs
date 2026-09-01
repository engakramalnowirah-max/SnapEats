using MediatR;

namespace SnapEats.Application.Features.MenuItem.Commands.DeleteMenuItem;

public sealed record DeleteMenuItemCommand : IRequest
{
    public int Id { get; init; }
}

