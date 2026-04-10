namespace RussianMilkApp.Models;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal FatContent { get; set; }
    public string PackageType { get; set; } = string.Empty;

    public override string ToString() => $"{ProductName} ({FatContent:0.##}% , {PackageType})";
}
