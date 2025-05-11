using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            // Категорії
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Продукти", Type = "витрата" },
                    new Category { Name = "Зарплата", Type = "дохід" },
                    new Category { Name = "Транспорт", Type = "витрата" }
                );
            }

            // Баланси
            if (!context.Balances.Any())
            {
                context.Balances.Add(
                    new Balance { Name = "Готівка", Amount = 0, Icon = "💵" }
                );
            }

            context.SaveChanges();
        }
    }
}
