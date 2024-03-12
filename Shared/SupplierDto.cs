namespace Shared;
public class SupplierDto
{
    public int Id { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierEmail { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public string CountryCodeAndName { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateOnly UpdatedDate { get; set;}
}
