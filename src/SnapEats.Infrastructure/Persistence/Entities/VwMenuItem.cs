using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class VwMenuItem
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool? IsAvailable { get; set; }
}
