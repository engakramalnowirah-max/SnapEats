using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SnapEats.AdminMVC.ViewModels;

public class OrderViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("customerId")]
    public int CustomerId { get; set; }

    [JsonProperty("customerName")]
    [Display(Name = "العميل")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonProperty("orderDate")]
    [Display(Name = "تاريخ الطلب")]
    public DateTime OrderDate { get; set; }

    [JsonProperty("status")]
    [Display(Name = "الحالة")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("totalAmount")]
    [Display(Name = "المجموع")]
    public decimal TotalAmount { get; set; }

    [JsonProperty("itemCount")]
    [Display(Name = "عدد العناصر")]
    public int ItemCount { get; set; }

    public string TotalFormatted => TotalAmount.ToString("N2");
    public string OrderDateFormatted => OrderDate.ToString("dd/MM/yyyy HH:mm");
    public string StatusText => Status switch
    {
        "Pending" => "قيد الانتظار",
        "Confirmed" => "مؤكد",
        "Preparing" => "قيد التحضير",
        "OutForDelivery" => "قيد التوصيل",
        "Delivered" => "تم التوصيل",
        "Cancelled" => "ملغي",
        _ => Status
    };
    public string StatusClass => Status switch
    {
        "Pending" => "bg-warning text-dark",
        "Confirmed" => "bg-info text-dark",
        "Preparing" => "bg-primary",
        "OutForDelivery" => "bg-secondary",
        "Delivered" => "bg-success",
        "Cancelled" => "bg-danger",
        _ => "bg-secondary"
    };
}

public class OrderDetailViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("customerId")]
    public int CustomerId { get; set; }

    [JsonProperty("customerName")]
    [Display(Name = "العميل")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonProperty("orderDate")]
    [Display(Name = "تاريخ الطلب")]
    public DateTime OrderDate { get; set; }

    [JsonProperty("status")]
    [Display(Name = "الحالة")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("totalAmount")]
    [Display(Name = "المجموع")]
    public decimal TotalAmount { get; set; }

    [JsonProperty("items")]
    public List<OrderItemViewModel> Items { get; set; } = new();

    public string TotalFormatted => TotalAmount.ToString("N2");
    public string OrderDateFormatted => OrderDate.ToString("dd/MM/yyyy HH:mm");
    public string StatusText => Status switch
    {
        "Pending" => "قيد الانتظار",
        "Confirmed" => "مؤكد",
        "Preparing" => "قيد التحضير",
        "OutForDelivery" => "قيد التوصيل",
        "Delivered" => "تم التوصيل",
        "Cancelled" => "ملغي",
        _ => Status
    };
    public string StatusClass => Status switch
    {
        "Pending" => "bg-warning text-dark",
        "Confirmed" => "bg-info text-dark",
        "Preparing" => "bg-primary",
        "OutForDelivery" => "bg-secondary",
        "Delivered" => "bg-success",
        "Cancelled" => "bg-danger",
        _ => "bg-secondary"
    };
}

public class OrderItemViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("menuItemName")]
    public string MenuItemName { get; set; } = string.Empty;

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("unitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonProperty("subTotal")]
    public decimal SubTotal { get; set; }

    public string UnitPriceFormatted => UnitPrice.ToString("N2");
    public string SubTotalFormatted => SubTotal.ToString("N2");
}

public class UpdateOrderStatusViewModel
{
    [Required]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "الحالة مطلوبة")]
    [Display(Name = "الحالة")]
    public string Status { get; set; } = string.Empty;

    public string? CurrentStatus { get; set; }
}

public class PagedResultViewModel<T>
{
    [JsonProperty("items")]
    public List<T> Items { get; set; } = new();

    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    [JsonProperty("totalPages")]
    public int TotalPages { get; set; }

    [JsonProperty("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }

    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }
}

