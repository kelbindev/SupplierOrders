namespace Shared.Pagination;
public class SupplierRequestParameter : BaseRequestParameters
{
    public string SearchSupplierName
    {
        get
        {
            string colName = "supplierName";
            return GetColumnSearchValueBasedOnColumnName(colName);
        }
    }
    public string SearchSupplierEmail
    {
        get
        {
            string colName = "supplierEmail";
            return GetColumnSearchValueBasedOnColumnName(colName);
        }
    }
    public string SearchCountry
    {
        get
        {
            string colName = "countryCodeAndName";
            return GetColumnSearchValueBasedOnColumnName(colName);
        }
    }
}
