using System.ComponentModel.DataAnnotations;

namespace Shared.User;

public record UserLoginDto
{
    [Required]
    public string UserName { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
