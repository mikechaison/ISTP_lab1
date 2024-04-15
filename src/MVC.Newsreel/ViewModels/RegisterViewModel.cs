using System.ComponentModel.DataAnnotations;

namespace MVC.Newsreel.ViewModel;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Нікнейм")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Ім'я, прізвище")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Паролі не збігаються")]
    [Display(Name = "Підтвердити пароль")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}