using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class UserRefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required, MaxLength(50)]
    public string RefreshToken { get; set; }
    [Required]
    public DateTime RefreshTokenExpiry { get; set; }

    public User User { get; set; }

    public bool RefreshTokenExpired
    {
        get
        {
            return DateTime.Now > RefreshTokenExpiry;
        }
    }
}
