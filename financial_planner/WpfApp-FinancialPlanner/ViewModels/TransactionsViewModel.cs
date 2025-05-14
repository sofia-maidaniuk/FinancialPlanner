using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
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

        public ObservableCollection<TransactionGroup> GroupedTransactions { get; set; } = new();
        public string SearchText { get; set; } = "";
        public string SelectedType { get; set; } = "усі";
        public string SelectedCategory { get; set; } = "усі";
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public ObservableCollection<string> AvailableCategories { get; set; } = new();
        public ObservableCollection<string> AvailableTypes { get; set; } = new() { "усі", "дохід", "витрата" };

        public TransactionsViewModel(ITransactionRepository repository)
        {
            _repository = repository;
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

            // Групування
            var grouped = filtered
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new TransactionGroup
                {
                    Date = g.Key,
                    Transactions = g.OrderByDescending(t => t.Date).ToList()
                });

            GroupedTransactions.Clear();
            foreach (var group in grouped)
                GroupedTransactions.Add(group);

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
