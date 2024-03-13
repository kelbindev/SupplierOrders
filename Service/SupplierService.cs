using Contracts.Repository;
using Entities;
using OfficeOpenXml;
using Service.Contracts;
using Shared;
using Shared.Pagination;

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

    public async Task<PagedList<Supplier>> GetAllPaged(SupplierRequestParameter param, bool trackChanges = false)
    {
        return await _repository.GetAllPaged(param, trackChanges);
    }

    public async Task<DownloadFileDto> GetAllPagedExportToExcel(SupplierRequestParameter param)
    {
        var supplierList = await _repository.GetAllPagedExportToExcel(param);

        string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Excel";
        string fileName = string.Format("Supplier_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
        string fullPath = string.Format("{0}\\{1}", filePath, fileName);

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        using (var excel = new ExcelPackage(new FileInfo(fullPath)))
        {
            var sheet1 = excel.Workbook.Worksheets.Add("Supplier");

            WriteFloorProfitProjectTemplateSheet(sheet1, supplierList);
            await excel.SaveAsync();
        }

        MemoryStream ms = new();
        await using (FileStream file = new(fullPath, FileMode.Open, FileAccess.Read))
        {
            await file.CopyToAsync(ms);
            ms.Position = 0;
        }

        //if (File.Exists(fullPath))
        //    File.Delete(fullPath);

        return new DownloadFileDto(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    public async Task<ApiResponse> Update(Supplier supplier)
    {
        if (!await _repository.Exists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} does not exists");

        if (await _repository.SupplierNameAlreadyExists(supplier))
            return ApiResponse.FailResponse($"Supplier {supplier.SupplierName} already exists");

        await _repository.Update(supplier);
        return ApiResponse.SuccessResponse($"Supplier {supplier.SupplierName} updated");
    }

    private void WriteFloorProfitProjectTemplateSheet(ExcelWorksheet sheet, List<Supplier> supplierList)
    {
        //TEMPLATE HEADER
        var row1Header = new object[] { "SupplierName","SupplierEmail","Country Code", "Country Name","CreatedDate","CreatedBy","UpdatedDate","UpdatedBy" };
        
        List<object[]> rows = new List<object[]>();

        rows.Add(row1Header);

        foreach(var supplier in supplierList)
        {
            var obj = new object[] { supplier.SupplierName, supplier.SupplierEmail, supplier.Country.CountryCode, supplier.Country.CountryName, supplier.CreatedDate, supplier.CreatedBy, supplier.UpdatedDate, supplier.UpdatedBy };
            rows.Add(obj);
        }

        sheet.Cells[1, 1].LoadFromArrays(rows);
    }
}
