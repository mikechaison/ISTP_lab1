using System.ComponentModel.DataAnnotations;
namespace MVC.Newsreel.ViewModel;
public class LoginViewModel
{
    [Required]
    [Display(Name = "Нікнейм")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запам'ятати?")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}