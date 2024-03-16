using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.User;
public class UserRegisterDto
{
    [Required, DisplayName("User Name")]
    public string UserName { get; set; }
    [Required, DisplayName("Email"), EmailAddress]

    public string UserEmail { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
};
