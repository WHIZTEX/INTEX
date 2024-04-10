namespace INTEX.Models.Infrastructure;

public class ProductsFilter
{
    public string? Category { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public int NumResults { get; set; }
}