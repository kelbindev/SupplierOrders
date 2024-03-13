using System.ComponentModel;

namespace Shared;
public class SupplierDto
{
    public int Id { get; set; }
    [DisplayName("Supplier Name")]
    public string SupplierName { get; set; } = string.Empty;
    [DisplayName("Supplier Email")]
    public string SupplierEmail { get; set; } = string.Empty;
    public int CountryId { get; set; }
    [DisplayName("Country")]
    public string CountryCodeAndName { get; set; } = string.Empty;
    [DisplayName("Updated By")]
    public string UpdatedBy { get; set; } = string.Empty;
    [DisplayName("Updated Date")]
    public DateOnly UpdatedDate { get; set;}
}
