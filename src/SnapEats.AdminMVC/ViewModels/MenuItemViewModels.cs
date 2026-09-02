using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SnapEats.AdminMVC.ViewModels;

public class MenuItemViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    [Display(Name = "الاسم")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [JsonProperty("price")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [JsonProperty("imageUrl")]
    [Display(Name = "صورة")]
    public string? ImageUrl { get; set; }

    [JsonProperty("isAvailable")]
    [Display(Name = "متاح")]
    public bool IsAvailable { get; set; }

    [JsonProperty("categoryName")]
    [Display(Name = "التصنيف")]
    public string CategoryName { get; set; } = string.Empty;

    [JsonProperty("createdAt")]
    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; }

    public string PriceFormatted => Price.ToString("N2");
    public string StatusText => IsAvailable ? "متاح" : "غير متاح";
    public string StatusClass => IsAvailable ? "bg-success" : "bg-danger";
}

public class CreateMenuItemViewModel
{
    [Required(ErrorMessage = "التصنيف مطلوب")]
    [Display(Name = "التصنيف")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "اسم العنصر مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
    [Display(Name = "اسم العنصر")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0.01, 99999.99, ErrorMessage = "السعر يجب أن يكون بين 0.01 و 99999.99")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "رابط الصورة غير صالح")]
    [Display(Name = "رابط الصورة")]
    public string? ImageUrl { get; set; }

    [Display(Name = "متاح")]
    public bool IsAvailable { get; set; } = true;
}

public class EditMenuItemViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم العنصر مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
    [Display(Name = "اسم العنصر")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0.01, 99999.99, ErrorMessage = "السعر يجب أن يكون بين 0.01 و 99999.99")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "رابط الصورة غير صالح")]
    [Display(Name = "رابط الصورة")]
    public string? ImageUrl { get; set; }

    [Display(Name = "متاح")]
    public bool IsAvailable { get; set; } = true;
}

