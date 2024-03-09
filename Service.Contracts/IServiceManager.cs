namespace Service.Contracts;
public interface IServiceManager
{
    ISupplierService Supplier { get; }
    ICountryService Country { get; }
}
