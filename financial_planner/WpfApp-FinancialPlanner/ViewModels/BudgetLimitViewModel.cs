using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class BudgetLimitViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;

        public ObservableCollection<BudgetLimit> BudgetLimits { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();

        private BudgetLimit? _selectedLimit;
        public BudgetLimit? SelectedLimit
        {
            get => _selectedLimit;
            set
            {
                _selectedLimit = value;
                OnPropertyChanged(nameof(SelectedLimit));
            }
        }

        public BudgetLimitViewModel(AppDbContext context)
        {
            _context = context;
            LoadData();
        }

        public void LoadData()
        {
            var limits = _context.BudgetLimits.Include(bl => bl.Category).ToList();
            BudgetLimits.Clear();
            foreach (var limit in limits)
                BudgetLimits.Add(limit);

            Categories.Clear();
            foreach (var category in _context.Categories.ToList())
                Categories.Add(category);
        }

        public async Task AddLimitAsync(BudgetLimit newLimit)
        {
            _context.BudgetLimits.Add(newLimit);
            await _context.SaveChangesAsync();
            LoadData();

            await NotifyTransactionsToReloadAsync(); 
        }

        private async Task NotifyTransactionsToReloadAsync()
        {
            var transactionsVM = App.Services.GetRequiredService<TransactionsViewModel>();
            await transactionsVM.LoadAsync();
        }

        public void UpdateLimit(BudgetLimit updatedLimit)
        {
            var existing = _context.BudgetLimits.FirstOrDefault(b => b.Id == updatedLimit.Id);
            if (existing != null)
            {
                existing.CategoryId = updatedLimit.CategoryId;
                existing.LimitAmount = updatedLimit.LimitAmount;
                existing.Year = updatedLimit.Year;
                existing.Month = updatedLimit.Month;
                _context.SaveChanges();
                LoadData();
            }
        }

        public void DeleteLimit(BudgetLimit limit)
        {
            _context.BudgetLimits.Remove(limit);
            _context.SaveChanges();
            LoadData();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
