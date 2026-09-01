using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual CustomerOrder Order { get; set; } = null!;
}
