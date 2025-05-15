using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;


namespace ClassLibrary_FinancialPlanner.Repositories
{
    public class BudgetLimitRepository : IBudgetLimitRepository
    {
        private readonly AppDbContext _context;

        public BudgetLimitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetLimit>> GetAllAsync()
        {
            return await _context.BudgetLimits.Include(b => b.Category).ToListAsync();
        }

        public async Task<BudgetLimit?> GetByIdAsync(int id)
        {
            return await _context.BudgetLimits.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(BudgetLimit limit)
        {
            _context.BudgetLimits.Add(limit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BudgetLimit limit)
        {
            _context.BudgetLimits.Update(limit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.BudgetLimits.FindAsync(id);
            if (item != null)
            {
                _context.BudgetLimits.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
