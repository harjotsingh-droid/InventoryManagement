namespace InventoryManagement.Application.Services;

public class QuotationLineCalculation
{
    public decimal LineSubTotal { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal TotalAmount { get; init; }
}

public class QuotationCalculationResult
{
    public decimal SubTotal { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal TotalAmount { get; init; }
    public IList<QuotationLineCalculation> Lines { get; init; } = new List<QuotationLineCalculation>();
}

public static class QuotationCalculator
{
    public static QuotationLineCalculation CalculateLine(
        decimal quantity,
        decimal unitPrice,
        decimal discountPercent,
        decimal gstPercent)
    {
        var lineAmount = quantity * unitPrice;
        var discount = lineAmount * discountPercent / 100m;
        var lineSubTotal = lineAmount - discount;
        var taxAmount = lineSubTotal * gstPercent / 100m;
        var totalAmount = lineSubTotal + taxAmount;

        return new QuotationLineCalculation
        {
            LineSubTotal = Math.Round(lineSubTotal, 2),
            TaxAmount = Math.Round(taxAmount, 2),
            TotalAmount = Math.Round(totalAmount, 2)
        };
    }

    public static QuotationCalculationResult Calculate(
        IEnumerable<(decimal Quantity, decimal UnitPrice, decimal DiscountPercent, decimal GstPercent)> lines,
        decimal quotationDiscountAmount = 0)
    {
        var lineResults = lines
            .Select(l => CalculateLine(l.Quantity, l.UnitPrice, l.DiscountPercent, l.GstPercent))
            .ToList();

        var subTotal = Math.Round(lineResults.Sum(l => l.LineSubTotal), 2);
        var taxAmount = Math.Round(lineResults.Sum(l => l.TaxAmount), 2);
        var totalAmount = Math.Round(subTotal + taxAmount - quotationDiscountAmount, 2);

        return new QuotationCalculationResult
        {
            SubTotal = subTotal,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Lines = lineResults
        };
    }
}
