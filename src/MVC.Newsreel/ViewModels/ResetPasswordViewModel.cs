using System.ComponentModel.DataAnnotations;
namespace MVC.Newsreel.ViewModel;
public class ResetPasswordModel
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }

    [Required, DataType(DataType.Password)]
    [Display(Name = "Новий пароль")]
    public string NewPassword { get; set; }

    [Required, DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Паролі не збігаються")]
    [Display(Name = "Підтвердити пароль")]
    public string ConfirmNewPassword { get; set; }

    public bool IsSuccess { get; set; }
}