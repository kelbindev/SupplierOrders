using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.Pagination;

namespace SupplierOrders.Controllers;
public class SupplierController(IServiceManager _service) : Controller
{
    // GET: SupplierController
    public ActionResult Index()
    {
        return View();
    }

    // GET: SupplierController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        if (id == 0) return NotFound();

        var supplier = await _service.Supplier.Get(id);

        if (supplier is null) return NotFound();

        return View(supplier);
    }

    // GET: SupplierController/Create
    public async Task<ActionResult> Create()
    {
        ViewBag.Country = await _service.Country.GetAll();

        return View();
    }

    // POST: SupplierController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Supplier supplier)
    {
        supplier.CreatedBy = "USER";
        supplier.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        supplier.UpdatedBy = "USER";
        supplier.UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await _service.Supplier.Add(supplier);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(supplier);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: SupplierController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        if (id == 0) return NotFound();

        ViewBag.Country = await _service.Country.GetAll();

        var supplier = await _service.Supplier.Get(id);

        if (supplier is null) return NotFound();

        return View(supplier);
    }

    // POST: SupplierController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Supplier supplier)
    {
        ViewBag.Country = await _service.Country.GetAll();

        supplier.UpdatedBy = "USER";
        supplier.UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await _service.Supplier.Update(supplier);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(supplier);
        }


        return RedirectToAction(nameof(Index));
    }

    // GET: SupplierController/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        if (id == 0) return NotFound();

        var supplier = await _service.Supplier.Get(id);

        if (supplier is null) return NotFound();

        return View(supplier);
    }

    // POST: SupplierController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(Supplier supplier)
    {
        var result = await _service.Supplier.Delete(supplier);

        if (!result.Success) {
            TempData["ErrorMessage"] = result.Message;
            return View(supplier);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<ActionResult> GetSupplierPaged(SupplierRequestParameter param)
    {
        var result = await _service.Supplier.GetAllPaged(param);

        var data = new List<SupplierDto>();

        foreach (var item in result)
        {
            var dto = new SupplierDto
            {
                Id = item.Id,
                SupplierEmail = item.SupplierEmail,
                SupplierName = item.SupplierName,
                CountryId = item.CountryId,
                CountryCodeAndName = item.Country.CountryCodeAndName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate
            };
            data.Add(dto);
        }

        return Json(new { data, metaData = result.MetaData });
    }
}
