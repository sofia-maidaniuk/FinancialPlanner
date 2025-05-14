using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class TransactionsViewModel
    {
        private readonly ITransactionRepository _repository;

        public ObservableCollection<TransactionGroup> GroupedTransactions { get; set; } = new();

        public TransactionsViewModel(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            var transactions = await _repository.GetAllAsync();

            var grouped = transactions
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new TransactionGroup
                {
                    Date = g.Key,
                    Transactions = g.OrderByDescending(t => t.Date).ToList()
                });

            GroupedTransactions.Clear();
            foreach (var group in grouped)
            {
                GroupedTransactions.Add(group);
            }
        }
    }
}