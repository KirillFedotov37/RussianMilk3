namespace RussianMilkApp.Models;

public class Warehouse
{
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public override string ToString() => $"{WarehouseName} — {Address}";
}
