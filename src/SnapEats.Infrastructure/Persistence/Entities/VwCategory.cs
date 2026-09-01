using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class VwCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? TotalItems { get; set; }
}
