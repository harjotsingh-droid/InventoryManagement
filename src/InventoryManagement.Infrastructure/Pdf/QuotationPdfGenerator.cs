using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Interfaces;
using InventoryManagement.Shared.Theme;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventoryManagement.Infrastructure.Pdf;

public class QuotationPdfGenerator : IQuotationPdfGenerator
{
    static QuotationPdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Generate(QuotationPdfContextDto context)
    {
        var settings = context.Settings;
        var quotation = context.Quotation;
        var primaryColor = ThemeHelper.NormalizePrimaryColor(settings.PrimaryColor);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Item().Background(primaryColor).Padding(12).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(settings.Profile.Name).FontSize(18).Bold().FontColor(Colors.White);
                            if (!string.IsNullOrWhiteSpace(settings.Profile.Tagline))
                            {
                                col.Item().Text(settings.Profile.Tagline).FontColor(Colors.White);
                            }
                        });
                        row.ConstantItem(180).AlignRight().Column(col =>
                        {
                            col.Item().Text("QUOTATION").FontSize(16).Bold().FontColor(Colors.White);
                            col.Item().Text(quotation.QuotationNumber).FontColor(Colors.White);
                        });
                    });

                    column.Item().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(settings.Profile.Address ?? string.Empty);
                            col.Item().Text($"{settings.Profile.City}, {settings.Profile.State} {settings.Profile.PinCode}");
                            col.Item().Text($"GSTIN: {settings.Profile.GstNumber}");
                            col.Item().Text($"PAN: {settings.Profile.PanNumber}");
                            col.Item().Text($"Email: {settings.Profile.Email} | Mobile: {settings.Profile.Mobile}");
                        });
                        row.ConstantItem(180).Column(col =>
                        {
                            col.Item().Text($"Date: {quotation.QuotationDate:dd MMM yyyy}");
                            col.Item().Text($"Valid Until: {quotation.ValidUntil:dd MMM yyyy}");
                            col.Item().Text($"Customer: {quotation.CustomerName}");
                        });
                    });
                });

                page.Content().PaddingVertical(15).Column(column =>
                {
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(primaryColor).Padding(5).Text("Product").FontColor(Colors.White).Bold();
                            header.Cell().Background(primaryColor).Padding(5).Text("Qty").FontColor(Colors.White).Bold();
                            header.Cell().Background(primaryColor).Padding(5).Text("Price").FontColor(Colors.White).Bold();
                            header.Cell().Background(primaryColor).Padding(5).Text("Disc %").FontColor(Colors.White).Bold();
                            header.Cell().Background(primaryColor).Padding(5).Text("GST %").FontColor(Colors.White).Bold();
                            header.Cell().Background(primaryColor).Padding(5).AlignRight().Text("Total").FontColor(Colors.White).Bold();
                        });

                        foreach (var line in quotation.Lines)
                        {
                            table.Cell().Padding(5).Text($"{line.ProductName} ({line.ProductSku})");
                            table.Cell().Padding(5).Text(line.Quantity.ToString("0.##"));
                            table.Cell().Padding(5).Text(line.UnitPrice.ToString("0.00"));
                            table.Cell().Padding(5).Text(line.DiscountPercent.ToString("0.##"));
                            table.Cell().Padding(5).Text(line.GstPercent.ToString("0.##"));
                            table.Cell().Padding(5).AlignRight().Text(line.TotalAmount.ToString("0.00"));
                        }
                    });

                    column.Item().PaddingTop(15).AlignRight().Width(220).Background(primaryColor).Padding(10).Column(totals =>
                    {
                        totals.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Subtotal").FontColor(Colors.White);
                            r.ConstantItem(80).AlignRight().Text(quotation.SubTotal.ToString("0.00")).FontColor(Colors.White);
                        });
                        totals.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Tax").FontColor(Colors.White);
                            r.ConstantItem(80).AlignRight().Text(quotation.TaxAmount.ToString("0.00")).FontColor(Colors.White);
                        });
                        if (quotation.DiscountAmount > 0)
                        {
                            totals.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Discount").FontColor(Colors.White);
                                r.ConstantItem(80).AlignRight().Text($"-{quotation.DiscountAmount:0.00}").FontColor(Colors.White);
                            });
                        }
                        totals.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Grand Total").Bold().FontColor(Colors.White);
                            r.ConstantItem(80).AlignRight().Text(quotation.TotalAmount.ToString("0.00")).Bold().FontColor(Colors.White);
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(quotation.Notes))
                    {
                        column.Item().PaddingTop(15).Text("Notes").Bold();
                        column.Item().Text(quotation.Notes);
                    }

                    column.Item().PaddingTop(15).Text("Terms & Conditions").Bold();
                    column.Item().Text(settings.InvoiceTerms);

                    column.Item().PaddingTop(10).Text(settings.InvoiceFooter).Italic();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }
}
