using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers;

[Authorize]
public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _customerService.GetAllAsync(search);
        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage;
            return View(Array.Empty<CustomerDto>());
        }

        ViewBag.Search = search;
        return View(result.Data);
    }

    public IActionResult Create()
    {
        return View(new CustomerFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _customerService.CreateAsync(new CreateCustomerDto
        {
            Name = model.Name,
            Code = model.Code,
            Mobile = model.Mobile,
            City = model.City,
            State = model.State,
            Address = model.Address
        });

        if (!result.Success)
        {
            AddErrors(result);
            return View(model);
        }

        TempData["Success"] = "Customer created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _customerService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.ErrorMessage ?? "Customer not found.";
            return RedirectToAction(nameof(Index));
        }

        var customer = result.Data;
        return View(new CustomerFormViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Code = customer.Code,
            Mobile = customer.Mobile,
            City = customer.City,
            State = customer.State,
            Address = customer.Address
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _customerService.UpdateAsync(new UpdateCustomerDto
        {
            Id = model.Id,
            Name = model.Name,
            Code = model.Code,
            Mobile = model.Mobile,
            City = model.City,
            State = model.State,
            Address = model.Address
        });

        if (!result.Success)
        {
            AddErrors(result);
            return View(model);
        }

        TempData["Success"] = "Customer updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    private void AddErrors(Application.Common.ServiceResult result)
    {
        if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
        }

        if (result.ValidationErrors == null)
        {
            return;
        }

        foreach (var error in result.ValidationErrors)
        {
            ModelState.AddModelError(error.Key, string.Join(" ", error.Value));
        }
    }
}
