using System.ComponentModel.DataAnnotations;

namespace SnapEats.AdminMVC.ViewModels;

public class DashboardViewModel
{
    [Display(Name = "إجمالي الطلبات")]
    public int TotalOrders { get; set; }

    [Display(Name = "إجمالي التصنيفات")]
    public int TotalCategories { get; set; }

    [Display(Name = "إجمالي عناصر القائمة")]
    public int TotalMenuItems { get; set; }

    [Display(Name = "إجمالي العملاء")]
    public int TotalCustomers { get; set; }

    [Display(Name = "الطلبات المعلقة")]
    public int PendingOrders { get; set; }

    [Display(Name = "الطلبات قيد التوصيل")]
    public int DeliveringOrders { get; set; }

    [Display(Name = "الطلبات المكتملة اليوم")]
    public int CompletedToday { get; set; }

    public List<OrderViewModel> RecentOrders { get; set; } = new();
}

