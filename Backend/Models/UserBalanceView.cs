namespace Split.Models;

public class UserBalanceView
{
    public string User { get; set; } = "";
    public decimal TotalSpent { get; set; }
    public decimal Average { get; set; }
    public decimal Net { get; set; }
    public string Formula { get; set; } = "";
}