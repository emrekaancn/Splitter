namespace Split.Models;

public class SettlementResult
{
    public List<TransferView> Summary { get; set; } = new();
    public List<UserBalanceView> Balances { get; set; } = new();
}