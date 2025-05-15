using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Interfaces
{
    public interface IBudgetLimitRepository
    {
        Task<List<BudgetLimit>> GetAllAsync();
        Task<BudgetLimit?> GetByIdAsync(int id);
        Task AddAsync(BudgetLimit limit);
        Task UpdateAsync(BudgetLimit limit);
        Task DeleteAsync(int id);
    }
}
