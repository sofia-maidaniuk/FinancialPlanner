using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary_FinancialPlanner.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            // Перевірка на дублікат — залишаємо
            var duplicateExists = await _context.Transactions
                .AnyAsync(t =>
                    t.Description == transaction.Description &&
                    t.Amount == transaction.Amount &&
                    t.Date.Date == transaction.Date.Date &&
                    t.CategoryId == transaction.CategoryId);

            if (duplicateExists)
            {
                Console.WriteLine($"Схожа транзакція вже існує: {transaction.Description} на {transaction.Date:d}");
                return;
            }

            // Зміна балансу
            var balance = await _context.Balances.FindAsync(transaction.BalanceId);
            if (balance != null)
            {
                if (transaction.Type.ToLower() == "витрата")
                {
                    balance.Amount -= transaction.Amount;
                }
                else if (transaction.Type.ToLower() == "дохід")
                {
                    balance.Amount += transaction.Amount;
                }
            }

            // Збереження транзакції
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction != null)
            {
                var balance = transaction.Balance;

                // Відновлюємо баланс
                if (balance != null)
                {
                    if (transaction.Type.ToLower() == "витрата")
                    {
                        balance.Amount += transaction.Amount;
                    }
                    else if (transaction.Type.ToLower() == "дохід")
                    {
                        balance.Amount -= transaction.Amount;
                    }
                }

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Balance)
                .ToListAsync();
        }


        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Transaction>> GetByCategoryAsync(string categoryName)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Balance)
                .Where(t => t.Category.Name == categoryName)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Balance)
                .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date)
                .ToListAsync();
        }

        public async Task UpdateAsync(Transaction updatedTransaction)
        {
            var existing = await _context.Transactions
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(t => t.Id == updatedTransaction.Id);

            if (existing != null)
            {
                var balance = existing.Balance;

                if (balance != null)
                {
                    // Відмінити стару суму
                    if (existing.Type.ToLower() == "витрата")
                        balance.Amount += existing.Amount;
                    else if (existing.Type.ToLower() == "дохід")
                        balance.Amount -= existing.Amount;

                    // Застосувати нову суму
                    if (updatedTransaction.Type.ToLower() == "витрата")
                        balance.Amount -= updatedTransaction.Amount;
                    else if (updatedTransaction.Type.ToLower() == "дохід")
                        balance.Amount += updatedTransaction.Amount;
                }

                // Оновити інші поля
                existing.Description = updatedTransaction.Description;
                existing.Amount = updatedTransaction.Amount;
                existing.CategoryId = updatedTransaction.CategoryId;
                existing.Type = updatedTransaction.Type;
                existing.Date = updatedTransaction.Date;

                await _context.SaveChangesAsync();
            }
        }
    }
}
