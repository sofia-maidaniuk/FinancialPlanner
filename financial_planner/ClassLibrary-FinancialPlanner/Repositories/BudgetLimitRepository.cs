using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ClassLibrary_FinancialPlanner.Repositories
{
    public class BudgetLimitRepository : GenericRepository<BudgetLimit>, IBudgetLimitRepository
    {
        public BudgetLimitRepository(AppDbContext context)
            : base(context) { }

        public override Task<List<BudgetLimit>> GetAllAsync()
            => _dbSet.Include(b => b.Category)
                     .AsNoTracking()
                     .ToListAsync();

        public override Task<BudgetLimit?> GetByIdAsync(int id)
            => _dbSet.Include(b => b.Category)
                     .FirstOrDefaultAsync(b => b.Id == id);
    }
}