using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context)
            : base(context) { }

        public override async Task AddAsync(Transaction transaction)
        {
            bool duplicateExists = await _dbSet.AnyAsync(t =>
                t.Description == transaction.Description &&
                t.Amount == transaction.Amount &&
                t.Date.Date == transaction.Date.Date &&
                t.CategoryId == transaction.CategoryId);

            if (duplicateExists)
            {
                Console.WriteLine($"Similar transaction exists: {transaction.Description} on {transaction.Date:d}");
                return;
            }

            var balance = await _context.Balances.FindAsync(transaction.BalanceId);
            if (balance != null)
            {
                if (transaction.Type.Equals("витрата", StringComparison.OrdinalIgnoreCase))
                    balance.Amount -= transaction.Amount;
                else if (transaction.Type.Equals("дохід", StringComparison.OrdinalIgnoreCase))
                    balance.Amount += transaction.Amount;
            }

            _dbSet.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var transaction = await _dbSet
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction != null)
            {
                var balance = transaction.Balance;
                if (balance != null)
                {
                    if (transaction.Type.Equals("витрата", StringComparison.OrdinalIgnoreCase))
                        balance.Amount += transaction.Amount;
                    else if (transaction.Type.Equals("дохід", StringComparison.OrdinalIgnoreCase))
                        balance.Amount -= transaction.Amount;
                }

                _dbSet.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public override Task<List<Transaction>> GetAllAsync()
            => _dbSet.Include(t => t.Category)
                     .Include(t => t.Balance)
                     .ToListAsync();

        public override Task<Transaction?> GetByIdAsync(int id)
            => _dbSet.Include(t => t.Category)
                     .Include(t => t.Balance)
                     .FirstOrDefaultAsync(t => t.Id == id);

        public Task<List<Transaction>> GetByCategoryAsync(string categoryName)
            => _dbSet.Include(t => t.Category)
                     .Include(t => t.Balance)
                     .Where(t => t.Category.Name == categoryName)
                     .ToListAsync();

        public Task<List<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
            => _dbSet.Include(t => t.Category)
                     .Include(t => t.Balance)
                     .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date)
                     .ToListAsync();
    }
}