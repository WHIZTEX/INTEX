namespace INTEX.Models.Infrastructure;

public class ProductsFilter
{
    public string[] Category { get; } = Array.Empty<string>();
    public string[] PrimaryColor { get; } = Array.Empty<string>();
    public string[] SecondaryColor { get; } = Array.Empty<string>();
    public int ProductsPerPage => 20;
}