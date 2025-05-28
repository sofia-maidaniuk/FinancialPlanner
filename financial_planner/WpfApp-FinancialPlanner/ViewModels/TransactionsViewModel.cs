using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class TransactionsViewModel : INotifyPropertyChanged
    {
        private readonly ITransactionRepository _repository;
        private readonly AppDbContext _context;

        public ObservableCollection<TransactionGroup> GroupedTransactions { get; set; } = new();
        public string SearchText { get; set; } = "";
        public string SelectedType { get; set; } = "усі";
        public string SelectedCategory { get; set; } = "усі";
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public ObservableCollection<string> AvailableCategories { get; set; } = new();
        public ObservableCollection<string> AvailableTypes { get; set; } = new() { "усі", "дохід", "витрата" };

        public TransactionsViewModel(ITransactionRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task LoadAsync()
        {
            var transactions = await _repository.GetAllAsync();

            // Пошук і фільтрація
            var filtered = transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(t => t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (SelectedType != "усі")
                filtered = filtered.Where(t => t.Type == SelectedType);

            if (SelectedCategory != "усі")
                filtered = filtered.Where(t => t.Category.Name == SelectedCategory);

            if (DateFrom.HasValue)
                filtered = filtered.Where(t => t.Date.Date >= DateFrom.Value.Date);

            if (DateTo.HasValue)
                filtered = filtered.Where(t => t.Date.Date <= DateTo.Value.Date);

            // Застосувати групування до відфільтрованих транзакцій
            ApplyGrouping(filtered);

            // Категорії для фільтра
            var uniqueCategories = transactions
                .Select(t => t.Category.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            AvailableCategories.Clear();
            AvailableCategories.Add("усі");
            foreach (var cat in uniqueCategories)
                AvailableCategories.Add(cat);

            OnPropertyChanged(nameof(AvailableCategories));
        }

        private void ApplyGrouping(IQueryable<Transaction> transactions)
        {
            var groups = transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new TransactionGroup
                {
                    Date = g.Key,
                    Transactions = g.OrderByDescending(t => t.Date).ToList()
                })
                .OrderByDescending(g => g.Date);

            GroupedTransactions.Clear();
            foreach (var group in groups)
                GroupedTransactions.Add(group);
        }

        public async Task DeleteAndReloadAsync(int transactionId)
        {
            var transaction = await _repository.GetByIdAsync(transactionId);
            await _repository.DeleteAsync(transactionId);

            // Оновити ліміти, якщо це витрата
            if (transaction?.Type == "витрата")
            {
                var month = transaction.Date.Month;
                var year = transaction.Date.Year;
                var categoryId = transaction.CategoryId;

                var limit = _context.BudgetLimits
                    .FirstOrDefault(l => l.CategoryId == categoryId && l.Month == month && l.Year == year);

                if (limit != null)
                {
                    await _context.SaveChangesAsync();
                }
            }

            await LoadAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
