using InventoryManagement.Application.DTOs;
using InventoryManagement.Application.Services;
using InventoryManagement.Shared.Constants;

namespace InventoryManagement.Application.Tests;

public class QuotationCalculatorTests
{
    [Fact]
    public void Calculate_WithKnownLineItems_ProducesCorrectTotals()
    {
        var lines = new List<(decimal Quantity, decimal UnitPrice, decimal DiscountPercent, decimal GstPercent)>
        {
            (2m, 100m, 10m, 18m),
            (1m, 500m, 0m, 12m)
        };

        var result = QuotationCalculator.Calculate(lines, quotationDiscountAmount: 50m);

        Assert.Equal(680m, result.SubTotal);
        Assert.Equal(92.40m, result.TaxAmount);
        Assert.Equal(722.40m, result.TotalAmount);
        Assert.Equal(2, result.Lines.Count);
        Assert.Equal(212.40m, result.Lines[0].TotalAmount);
        Assert.Equal(560m, result.Lines[1].TotalAmount);
    }

    [Fact]
    public void CalculateLine_WithDiscountAndGst_ComputesLineTotals()
    {
        var line = QuotationCalculator.CalculateLine(quantity: 3m, unitPrice: 50m, discountPercent: 20m, gstPercent: 18m);

        Assert.Equal(120m, line.LineSubTotal);
        Assert.Equal(21.60m, line.TaxAmount);
        Assert.Equal(141.60m, line.TotalAmount);
    }

    [Fact]
    public void CalculateLine_WithFullDiscount_HasZeroTax()
    {
        var line = QuotationCalculator.CalculateLine(quantity: 5m, unitPrice: 100m, discountPercent: 100m, gstPercent: 18m);

        Assert.Equal(0m, line.LineSubTotal);
        Assert.Equal(0m, line.TaxAmount);
        Assert.Equal(0m, line.TotalAmount);
    }

    [Fact]
    public void Calculate_WithQuotationDiscount_SubtractsFromDocumentTotal()
    {
        var lines = new List<(decimal Quantity, decimal UnitPrice, decimal DiscountPercent, decimal GstPercent)>
        {
            (1m, 200m, 0m, 18m)
        };

        var result = QuotationCalculator.Calculate(lines, quotationDiscountAmount: 50m);

        Assert.Equal(200m, result.SubTotal);
        Assert.Equal(36m, result.TaxAmount);
        Assert.Equal(186m, result.TotalAmount);
    }
}
