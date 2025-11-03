using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Onatrix_Umbraco.ViewModels;

public class CallbackFormViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email address")]
    [RegularExpression(
    @"^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}|(?:\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-zA-Z-]*[a-zA-Z]:.+)\]))$",
    ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone is required")]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Please select an option")]
    public string SelectedOption { get; set; } = null!;

    [BindNever]
    public IEnumerable<string> Options { get; set; } = [];
}
