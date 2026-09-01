using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class VwOrder
{
    public int OrderId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal? TotalAmount { get; set; }
}
