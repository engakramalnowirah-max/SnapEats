using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class VwOrderInvoice
{
    public int OrderId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public decimal? TotalAmount { get; set; }
}
