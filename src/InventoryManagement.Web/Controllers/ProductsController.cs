using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _productService.GetAllAsync(search);
        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage;
            return View(Array.Empty<ProductDto>());
        }

        ViewBag.Search = search;
        return View(result.Data);
    }

    public IActionResult Create()
    {
        return View(new ProductFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _productService.CreateAsync(new CreateProductDto
        {
            Name = model.Name,
            Sku = model.Sku,
            Barcode = model.Barcode,
            SellingPrice = model.SellingPrice,
            GstPercent = model.GstPercent,
            CurrentStock = model.CurrentStock
        });

        if (!result.Success)
        {
            AddErrors(result);
            return View(model);
        }

        TempData["Success"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.ErrorMessage ?? "Product not found.";
            return RedirectToAction(nameof(Index));
        }

        var product = result.Data;
        return View(new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku,
            Barcode = product.Barcode,
            SellingPrice = product.SellingPrice,
            GstPercent = product.GstPercent,
            CurrentStock = product.CurrentStock
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _productService.UpdateAsync(new UpdateProductDto
        {
            Id = model.Id,
            Name = model.Name,
            Sku = model.Sku,
            Barcode = model.Barcode,
            SellingPrice = model.SellingPrice,
            GstPercent = model.GstPercent,
            CurrentStock = model.CurrentStock
        });

        if (!result.Success)
        {
            AddErrors(result);
            return View(model);
        }

        TempData["Success"] = "Product updated successfully.";
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
