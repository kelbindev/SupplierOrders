using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities;
public class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(2)]
    [MinLength(2)]
    public string CountryCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string CountryName { get; set; }

    public ICollection<Supplier> Suppliers { get; set; }
}
