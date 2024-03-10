using Contracts.Repository;
using Entities;
using Service.Contracts;
using Shared;

namespace Service;
internal sealed class SupplierService(ISupplierRepository _repository) : ISupplierService
{
    public async Task<ApiResponse> Add(Supplier supplier)
    {
        if (await _repository.Exists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} already exists");

        await _repository.Add(supplier);
        return ApiResponse.SuccessResponse($"Supplier {supplier.SupplierName} Added");
    }

    public async Task<ApiResponse> Delete(Supplier supplier)
    {
        if (!await _repository.Exists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} does not exists");

        await _repository.Delete(supplier);
        return ApiResponse.SuccessResponse($"Supplier {supplier.SupplierName} deleted");
    }

    public async Task<Supplier> Get(int id)
    {
        return await _repository.Get(id);
    }

    public async Task<IEnumerable<Supplier>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<ApiResponse> Update(Supplier supplier)
    {
        if (!await _repository.Exists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} does not exists");

        await _repository.Update(supplier);
        return ApiResponse.SuccessResponse($"Supplier {supplier.SupplierName} updated");
    }
}
