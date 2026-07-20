namespace InventoryManagement.Shared.Theme;

public static class ThemeHelper
{
    public const string DefaultPrimaryColor = "#2563eb";

    public static string NormalizePrimaryColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return DefaultPrimaryColor;
        }

        var trimmed = color.Trim();
        return trimmed.StartsWith('#') && (trimmed.Length == 7 || trimmed.Length == 4)
            ? trimmed
            : DefaultPrimaryColor;
    }
}
