namespace RussianMilkApp.Models;

public class BatchView
{
    public int BatchId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public DateTime ProductionDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Quantity { get; set; }
    public int DaysLeft => (ExpirationDate.Date - DateTime.Today).Days;
    public string Status => DaysLeft < 0 ? "Просрочено" : DaysLeft <= 3 ? "Срок истекает" : "Актуально";
}
