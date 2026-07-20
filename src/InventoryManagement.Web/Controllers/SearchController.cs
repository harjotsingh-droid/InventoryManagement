using InventoryManagement.Application.Interfaces;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<IActionResult> Index(string? q)
    {
        var model = new SearchViewModel { Query = q ?? string.Empty };

        if (string.IsNullOrWhiteSpace(q))
        {
            return View(model);
        }

        var result = await _searchService.SearchAsync(q);
        if (!result.Success)
        {
            ViewBag.Error = result.ErrorMessage;
            return View(model);
        }

        ViewBag.Results = result.Data;
        return View(model);
    }
}
