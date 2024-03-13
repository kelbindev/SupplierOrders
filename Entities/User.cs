using System.ComponentModel.DataAnnotations;

namespace Entities;
public class User : CommonField
{
    [Key, Required, MinLength(6)]
    public int UserId {  get; set; }
    [Required,MaxLength(50)]
    public string UserName { get; set; }
    [Required, EmailAddress, MaxLength(200)]
    public string UserEmail {  get; set; }
    [Required, MaxLength]
    public byte[] Password {  get; set; }
    [Required,MaxLength(50)]
    public string PasswordSalt { get; set; }

    public UserRefreshToken RefreshToken { get; set; }
}
