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
}
