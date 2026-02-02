using System;
using System.Collections.Generic;
using System.Linq;
using Split.Models;

namespace Split.Services
{
    public class SplitSettlementService
    {
        // Kullanıcıların harcamalarını alıp kişi başı hesaplama
        public Dictionary<string, decimal> Calculate(List<User> users, List<Expense> expenses)
        {
            var userTotals = users.ToDictionary(u => u.Name, u => 0m);

           foreach (var expense in expenses)
{
        if (expense.User == null) continue; // User boşsa atla

        if (userTotals.ContainsKey(expense.User.Name))
        {
            userTotals[expense.User.Name] += expense.Amount;
    }
}

            // Toplam harcama ve kişi başı pay
            var total = userTotals.Values.Sum();
            var perPerson = total / users.Count;

            // Kullanıcıların alacak/verecekleri
            var balances = userTotals.ToDictionary(kvp => kvp.Key, kvp => kvp.Value - perPerson);

            return balances;
        }

        // Kimin kime ne kadar ödeyeceğini gösteren detaylı hesaplama
        public List<Participant> CalculateTransfersWithNames(List<User> users, List<Expense> expenses)
        {
            var balances = Calculate(users, expenses);

            var debtors = balances
                .Where(kvp => kvp.Value < 0)
                .Select(kvp => new Participant { Name = kvp.Key, Amount = Math.Abs(kvp.Value) })
                .OrderBy(p => p.Amount)
                .ToList();

            var creditors = balances
                .Where(kvp => kvp.Value > 0)
                .Select(kvp => new Participant { Name = kvp.Key, Amount = kvp.Value })
                .OrderByDescending(p => p.Amount)
                .ToList();

            var settlements = new List<Participant>();

            int i = 0, j = 0;

            while (i < debtors.Count && j < creditors.Count)
            {
                var debtor = debtors[i];
                var creditor = creditors[j];

                var amount = Math.Min(debtor.Amount, creditor.Amount);

                settlements.Add(new Participant
                {
                    Name = $"{debtor.Name} -> {creditor.Name}",
                    Amount = amount
                });

                debtor.Amount -= amount;
                creditor.Amount -= amount;

                if (debtor.Amount == 0) i++;
                if (creditor.Amount == 0) j++;
            }

            return settlements;
        }
    }
}