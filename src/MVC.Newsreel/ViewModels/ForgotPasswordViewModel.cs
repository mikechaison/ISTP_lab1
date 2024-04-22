using System.ComponentModel.DataAnnotations;
namespace MVC.Newsreel.ViewModel;
public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email-адреса")]
    public string Email { get; set; }
    public bool EmailSent { get; set; }
}