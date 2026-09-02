using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SnapEats.AdminMVC.ViewModels;

public class CustomerViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("fullName")]
    [Display(Name = "الاسم")]
    public string FullName { get; set; } = string.Empty;

    [JsonProperty("email")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phone")]
    [Display(Name = "رقم الجوال")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("createdAt")]
    [Display(Name = "تاريخ التسجيل")]
    public DateTime CreatedAt { get; set; }

    public string CreatedAtFormatted =>
        CreatedAt > DateTime.MinValue
            ? CreatedAt.ToString("dd/MM/yyyy")
            : "-";
}

public class CustomerDetailViewModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("fullName")]
    [Display(Name = "الاسم")]
    public string FullName { get; set; } = string.Empty;

    [JsonProperty("email")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phone")]
    [Display(Name = "رقم الجوال")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("createdAt")]
    [Display(Name = "تاريخ التسجيل")]
    public DateTime CreatedAt { get; set; }
}

