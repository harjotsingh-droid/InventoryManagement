namespace InventoryManagement.Application.DTOs;

public class DashboardDto
{
    public int ProductCount { get; set; }
    public int CustomerCount { get; set; }
    public int TodaysQuotationCount { get; set; }
    public int PendingQuotationCount { get; set; }
}

public class GlobalSearchResultDto
{
    public IList<SearchItemDto> Products { get; set; } = new List<SearchItemDto>();
    public IList<SearchItemDto> Customers { get; set; } = new List<SearchItemDto>();
    public IList<SearchItemDto> Quotations { get; set; } = new List<SearchItemDto>();
}

public class SearchItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
