using Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;

namespace SupplierOrders.Controllers;
public class SupplierController(ISupplierService _service) : Controller
{
    // GET: SupplierController
    public async Task<ActionResult> Index()
    {
        return View(await _service.GetAll());
    }

    // GET: SupplierController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var supplier = await _service.Get(id);

        return View(supplier);
    }

    // GET: SupplierController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: SupplierController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(SupplierDto dto)
    {
        var supplier = new Supplier
        {
            SupplierName = dto.SupplierName,
            SupplierEmail = dto.SupplierEmail,
            CountryId = dto.CountryId,
            CreatedBy = dto.ActionBy
        };

        var result = await _service.Add(supplier);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: SupplierController/Edit/5
    public ActionResult Edit(int id)
    {
        if (id == 0) return NotFound();

        return View();
    }

    // POST: SupplierController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Id = id,
            SupplierName = dto.SupplierName,
            SupplierEmail = dto.SupplierEmail,
            CountryId = dto.CountryId,
            UpdatedBy = dto.ActionBy,
            UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var result = await _service.Update(supplier);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }


        return RedirectToAction(nameof(Index));
    }

    // GET: SupplierController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: SupplierController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Id = id,
            SupplierName = dto.SupplierName,
            SupplierEmail = dto.SupplierEmail,
            CountryId = dto.CountryId,
            UpdatedBy = dto.ActionBy,
            UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var result = await _service.Delete(supplier);

        if (!result.Success) {
            TempData["ErrorMessage"] = result.Message;
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
}
