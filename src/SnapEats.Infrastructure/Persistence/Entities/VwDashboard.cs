using System;
using System.Collections.Generic;

namespace SnapEats.Infrastructure.Persistence.Entities;

public partial class VwDashboard
{
    public int? TotalCategories { get; set; }

    public int? TotalMenuItems { get; set; }

    public int? TotalCustomers { get; set; }

    public int? TotalOrders { get; set; }

    public decimal? TotalSales { get; set; }
}
