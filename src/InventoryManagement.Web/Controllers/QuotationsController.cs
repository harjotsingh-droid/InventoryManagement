using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers;

[Authorize]
public class QuotationsController : Controller
{
    private readonly IQuotationService _quotationService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IQuotationPdfGenerator _pdfGenerator;

    public QuotationsController(
        IQuotationService quotationService,
        ICustomerService customerService,
        IProductService productService,
        IQuotationPdfGenerator pdfGenerator)
    {
        _quotationService = quotationService;
        _customerService = customerService;
        _productService = productService;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _quotationService.GetAllAsync();
        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage;
            return View(Array.Empty<QuotationListItemDto>());
        }

        return View(result.Data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _quotationService.GetByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.ErrorMessage ?? "Quotation not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateLookupsAsync();
        return View(new QuotationFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuotationFormViewModel model)
    {
        await PopulateLookupsAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _quotationService.CreateAsync(new CreateQuotationDto
        {
            CustomerId = model.CustomerId,
            QuotationDate = model.QuotationDate,
            ValidUntil = model.ValidUntil,
            DiscountAmount = model.DiscountAmount,
            Notes = model.Notes,
            Lines = model.Lines.Select(l => new QuotationLineInputDto
            {
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                UnitPrice = l.UnitPrice,
                DiscountPercent = l.DiscountPercent,
                GstPercent = l.GstPercent
            }).ToList()
        });

        if (!result.Success)
        {
            AddErrors(result);
            return View(model);
        }

        TempData["Success"] = "Quotation created successfully.";
        return RedirectToAction(nameof(Details), new { id = result.Data!.Id });
    }

    public async Task<IActionResult> DownloadPdf(int id)
    {
        var contextResult = await _quotationService.GetPdfContextAsync(id);
        if (!contextResult.Success || contextResult.Data == null)
        {
            TempData["Error"] = contextResult.ErrorMessage ?? "Unable to generate PDF.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var pdf = _pdfGenerator.Generate(contextResult.Data);
        var fileName = $"{contextResult.Data.Quotation.QuotationNumber}.pdf";
        return File(pdf, "application/pdf", fileName);
    }

    [HttpGet]
    public async Task<IActionResult> ProductDefaults(int productId)
    {
        var result = await _productService.GetByIdAsync(productId);
        if (!result.Success || result.Data == null)
        {
            return NotFound();
        }

        return Json(new
        {
            unitPrice = result.Data.SellingPrice,
            gstPercent = result.Data.GstPercent
        });
    }

    private async Task PopulateLookupsAsync()
    {
        var customers = await _customerService.GetAllAsync();
        var products = await _productService.GetAllAsync();

        ViewBag.Customers = new SelectList(customers.Data ?? [], "Id", "Name");
        ViewBag.Products = new SelectList(products.Data ?? [], "Id", "Name");
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
