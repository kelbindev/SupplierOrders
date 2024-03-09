using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
public class Supplier : CommonField
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string SupplierName { get; set; }

    [Required]
    [EmailAddress]
    public string SupplierEmail { get; set; }

    [Required]
    public int CountryId {  get; set; }

    public Country Country { get; set; }
}
