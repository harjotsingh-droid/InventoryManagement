namespace InventoryManagement.Application.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Address { get; set; }
}

public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Address { get; set; }
}

public class UpdateCustomerDto : CreateCustomerDto
{
    public int Id { get; set; }
}
