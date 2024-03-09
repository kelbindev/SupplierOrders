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

    public async Task<ApiResponse> Get(int id)
    {
        var supplier = await _repository.Get(id);
     
        if (supplier is null)
            return ApiResponse.FailResponse($"Supplier with ID:{id} not found");

        return ApiResponse.SuccessResponse(supplier);
    }

    public async Task<ApiResponse> GetAll()
    {
        var supplierList = await _repository.GetAll();
        return ApiResponse.SuccessResponse(supplierList);
    }

    public async Task<ApiResponse> Update(Supplier supplier)
    {
        if (!await _repository.Exists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} does not exists");

        await _repository.Update(supplier);
        return ApiResponse.SuccessResponse($"Supplier {supplier.SupplierName} updated");
    }
}
