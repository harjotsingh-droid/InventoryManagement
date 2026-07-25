using System.Net;
using System.Text;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Infrastructure.Pdf;
using InventoryManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Web.Tests;

public class QuotationsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public QuotationsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Login_WithSeededCredentials_RedirectsToDashboard()
    {
        await _factory.SeedTestDataAsync();

        var loginPage = await _client.GetAsync("/Account/Login");
        var loginHtml = await loginPage.Content.ReadAsStringAsync();
        var token = AntiforgeryHelper.ExtractToken(loginHtml);

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = "admin@demo.com",
            ["Password"] = "Admin@123",
            ["__RequestVerificationToken"] = token
        });

        var response = await _client.PostAsync("/Account/Login", form);

        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        var location = response.Headers.Location?.ToString() ?? string.Empty;
        Assert.True(location == "/" || location.Contains("/Home", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateQuotation_AfterLogin_PersistsAndRedirectsToDetails()
    {
        await _factory.SeedTestDataAsync();
        await AuthHelper.LoginAsync(_client);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var customerId = db.Customers.Select(c => c.Id).First();
        var productId = db.Products.Select(p => p.Id).First();
        var product = db.Products.First(p => p.Id == productId);

        var createPage = await _client.GetAsync("/Quotations/Create");
        createPage.EnsureSuccessStatusCode();
        var createHtml = await createPage.Content.ReadAsStringAsync();
        var token = AntiforgeryHelper.ExtractToken(createHtml);

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["CustomerId"] = customerId.ToString(),
            ["QuotationDate"] = DateTime.Today.ToString("yyyy-MM-dd"),
            ["ValidUntil"] = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd"),
            ["DiscountAmount"] = "0",
            ["Notes"] = "Integration test quotation",
            ["Lines[0].ProductId"] = productId.ToString(),
            ["Lines[0].Quantity"] = "2",
            ["Lines[0].UnitPrice"] = product.SellingPrice.ToString(),
            ["Lines[0].DiscountPercent"] = "0",
            ["Lines[0].GstPercent"] = product.GstPercent.ToString(),
            ["__RequestVerificationToken"] = token
        });

        var response = await _client.PostAsync("/Quotations/Create", form);

        Assert.Equal(HttpStatusCode.Found, response.StatusCode);
        Assert.Contains("/Quotations/Details/", response.Headers.Location?.ToString());

        var quotationCount = db.Quotations.Count();
        Assert.Equal(1, quotationCount);
    }

    [Fact]
    public async Task CreateQuotation_WithZeroQuantity_ReturnsValidationError()
    {
        await _factory.SeedTestDataAsync();
        await AuthHelper.LoginAsync(_client);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var customerId = db.Customers.Select(c => c.Id).First();
        var productId = db.Products.Select(p => p.Id).First();

        var createPage = await _client.GetAsync("/Quotations/Create");
        var createHtml = await createPage.Content.ReadAsStringAsync();
        var token = AntiforgeryHelper.ExtractToken(createHtml);

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["CustomerId"] = customerId.ToString(),
            ["QuotationDate"] = DateTime.Today.ToString("yyyy-MM-dd"),
            ["ValidUntil"] = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd"),
            ["DiscountAmount"] = "0",
            ["Lines[0].ProductId"] = productId.ToString(),
            ["Lines[0].Quantity"] = "0",
            ["Lines[0].UnitPrice"] = "100",
            ["Lines[0].DiscountPercent"] = "0",
            ["Lines[0].GstPercent"] = "18",
            ["__RequestVerificationToken"] = token
        });

        var response = await _client.PostAsync("/Quotations/Create", form);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(0, db.Quotations.Count());
    }
}

public class QuotationPdfGeneratorTests
{
    [Fact]
    public void Generate_WithDifferentCompanyNames_ProducesDifferentPdfOutput()
    {
        var generator = new QuotationPdfGenerator();
        var baseContext = BuildSampleContext();
        var contextA = CloneContext(baseContext);
        contextA.Settings.Profile.Name = "Acme Custom Branding Ltd";
        var contextB = CloneContext(baseContext);
        contextB.Settings.Profile.Name = "Beta Industrial Supplies";

        var pdfA = generator.Generate(contextA);
        var pdfB = generator.Generate(contextB);

        Assert.NotEmpty(pdfA);
        Assert.NotEmpty(pdfB);
        Assert.StartsWith("%PDF", Encoding.Latin1.GetString(pdfA));
        Assert.NotEqual(Convert.ToBase64String(pdfA), Convert.ToBase64String(pdfB));
    }

    [Fact]
    public void Generate_WithCustomTerms_ProducesValidPdfDocument()
    {
        var generator = new QuotationPdfGenerator();
        var context = BuildSampleContext();
        context.Settings.InvoiceTerms = "Payment due within 15 days of acceptance.";
        context.Settings.InvoiceFooter = "Thank you for your business.";

        var pdf = generator.Generate(context);

        Assert.True(pdf.Length > 1000);
        Assert.StartsWith("%PDF", Encoding.Latin1.GetString(pdf));
    }

    private static QuotationPdfContextDto BuildSampleContext() => new()
    {
        Settings = new CompanySettingsDto
        {
            Profile = new CompanyProfileDto
            {
                Name = "Demo Trading Co.",
                Address = "99 Test Industrial Park",
                City = "Ludhiana",
                State = "Punjab",
                PinCode = "141001",
                GstNumber = "03TEST1234E1Z5",
                PanNumber = "TEST1234E",
                Email = "billing@demo.test",
                Mobile = "+91 90000 00000"
            },
            PrimaryColor = "#dc2626",
            InvoiceTerms = "Net 30 days.",
            InvoiceFooter = "Authorized signatory."
        },
        Quotation = new QuotationDto
        {
            QuotationNumber = "QT-2026-0001",
            CustomerName = "Alpha Hardware Store",
            QuotationDate = new DateTime(2026, 7, 2),
            ValidUntil = new DateTime(2026, 8, 2),
            SubTotal = 100m,
            TaxAmount = 18m,
            TotalAmount = 118m,
            Lines =
            [
                new QuotationLineDto
                {
                    ProductName = "Steel Bolt M8",
                    ProductSku = "SB-M8-001",
                    Quantity = 1,
                    UnitPrice = 100m,
                    GstPercent = 18m,
                    TaxAmount = 18m,
                    TotalAmount = 118m
                }
            ]
        }
    };

    private static QuotationPdfContextDto CloneContext(QuotationPdfContextDto source) => new()
    {
        Settings = new CompanySettingsDto
        {
            Profile = new CompanyProfileDto
            {
                Name = source.Settings.Profile.Name,
                Address = source.Settings.Profile.Address,
                City = source.Settings.Profile.City,
                State = source.Settings.Profile.State,
                PinCode = source.Settings.Profile.PinCode,
                GstNumber = source.Settings.Profile.GstNumber,
                PanNumber = source.Settings.Profile.PanNumber,
                Email = source.Settings.Profile.Email,
                Mobile = source.Settings.Profile.Mobile
            },
            PrimaryColor = source.Settings.PrimaryColor,
            InvoiceTerms = source.Settings.InvoiceTerms,
            InvoiceFooter = source.Settings.InvoiceFooter
        },
        Quotation = source.Quotation
    };
}
