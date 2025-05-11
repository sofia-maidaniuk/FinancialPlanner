using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(int id);
        Task<List<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetByCategoryAsync(string categoryName);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }
}
