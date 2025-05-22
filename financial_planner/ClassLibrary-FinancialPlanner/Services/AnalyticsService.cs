using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Services
{
    public class CategoryExpense
    {
        public string CategoryName { get; set; } = "";
        public decimal Amount { get; set; }
        public string Icon { get; set; } = "❓";
        public bool IsOverLimit { get; set; }
    }

    public class AnalyticsService
    {
        private readonly AppDbContext _context;

        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryExpense>> GetExpensesByCategoryAsync(List<Transaction> transactions, DateTime? dateTo)
        {
            return await Task.Run(() =>
            {
                var year = dateTo?.Year ?? DateTime.Now.Year;
                var month = dateTo?.Month ?? DateTime.Now.Month;

                return transactions
                    .Where(t => t.Type.ToLower() == "витрата")
                    .GroupBy(t => t.Category)
                    .Select(g =>
                    {
                        var category = g.Key;
                        var amount = g.Sum(t => t.Amount);
                        var limit = _context.BudgetLimits
                            .FirstOrDefault(l => l.CategoryId == category!.Id && l.Year == year && l.Month == month);

                        return new CategoryExpense
                        {
                            CategoryName = category?.Name ?? "Без категорії",
                            Icon = category?.Icon ?? "❓",
                            Amount = amount,
                            IsOverLimit = limit != null && amount > limit.LimitAmount
                        };
                    }).ToList();
            });
        }
    }
}
