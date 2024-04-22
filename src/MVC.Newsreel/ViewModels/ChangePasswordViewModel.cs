using System.ComponentModel.DataAnnotations;
namespace MVC.Newsreel.ViewModel;
public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Поточний пароль")]
    public string CurrentPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Новий пароль")]
    public string NewPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Підтвердити пароль")]
    [Compare("NewPassword", ErrorMessage = "Паролі не збігаються")]
    public string ConfirmPassword { get; set; }
}