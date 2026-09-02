using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SnapEats.AdminMVC.ViewModels;

public class CategoryViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    [Display(Name = "الاسم")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [JsonProperty("menuItemCount")]
    [Display(Name = "عدد العناصر")]
    public int MenuItemCount { get; set; }

    [JsonProperty("createdAt")]
    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; }
}

public class CreateCategoryViewModel
{
    [Required(ErrorMessage = "اسم التصنيف مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
    [Display(Name = "اسم التصنيف")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }
}

public class EditCategoryViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم التصنيف مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
    [Display(Name = "اسم التصنيف")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }
}

