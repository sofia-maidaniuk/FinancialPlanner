using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary_FinancialPlanner.Models;
using ClassLibrary_FinancialPlanner.Interfaces;
using System.Threading.Tasks;

namespace WpfApp_FinancialPlanner.Views.transaction
{
    public partial class AddTransactionWindow : Window
    {
        private readonly ITransactionRepository _repository;
        private readonly ClassLibrary_FinancialPlanner.Data.AppDbContext _context;

        public Transaction? CreatedTransaction { get; private set; }
        public event Action<Transaction>? TransactionAdded;

        public AddTransactionWindow()
        {
            InitializeComponent();
            _repository = App.Services.GetRequiredService<ITransactionRepository>();
            _context = App.Services.GetRequiredService<ClassLibrary_FinancialPlanner.Data.AppDbContext>();

            BalanceComboBox.ItemsSource = _context.Balances.ToList();
            TypeComboBox.SelectedIndex = 0;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedType != null)
            {
                CategoryComboBox.ItemsSource = _context.Categories
                    .Where(c => c.Type == selectedType)
                    .ToList();
                CategoryComboBox.SelectedIndex = 0;
            }
        }

        private bool _isSaving = false;

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_isSaving) return;
            _isSaving = true;

            try
            {
                if (!ValidateAmount(out decimal amount)) return;
                if (!ValidateBalance(out Balance selectedBalance)) return;
                if (!ValidateCategory(out Category selectedCategory)) return;
                if (!ValidateDescription()) return;

                var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "витрата";
                var newTransaction = CreateTransactionFromInput(amount, selectedBalance, selectedCategory, selectedType);

                if (!CheckBudgetLimit(newTransaction, selectedCategory))
                {
                    _isSaving = false;
                    return;
                }

                await _repository.AddAsync(newTransaction);
                CreatedTransaction = newTransaction;
                TransactionAdded?.Invoke(newTransaction);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _isSaving = false;
            }
        }

        private bool ValidateAmount(out decimal amount)
        {
            if (!decimal.TryParse(AmountBox.Text, out amount))
            {
                MessageBox.Show("Будь ласка, введіть коректну суму.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateBalance(out Balance selectedBalance)
        {
            if (BalanceComboBox.SelectedItem is not Balance balance)
            {
                MessageBox.Show("Будь ласка, оберіть баланс.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                selectedBalance = null!;
                return false;
            }
            selectedBalance = balance;
            return true;
        }

        private bool ValidateCategory(out Category selectedCategory)
        {
            if (CategoryComboBox.SelectedItem is not Category category)
            {
                MessageBox.Show("Будь ласка, оберіть категорію.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                selectedCategory = null!;
                return false;
            }
            selectedCategory = category;
            return true;
        }

        private bool ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Будь ласка, введіть опис транзакції.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private Transaction CreateTransactionFromInput(decimal amount, Balance selectedBalance, Category selectedCategory, string selectedType)
        {
            return new Transaction
            {
                Description = DescriptionBox.Text.Trim(),
                Amount = amount,
                BalanceId = selectedBalance.Id,
                CategoryId = selectedCategory.Id,
                Type = selectedType,
                Date = DatePicker.SelectedDate ?? DateTime.Now
            };
        }

        private bool CheckBudgetLimit(Transaction transaction, Category selectedCategory)
        {
            if (transaction.Type.ToLower() != "витрата")
                return true;

            var month = transaction.Date.Month;
            var year = transaction.Date.Year;
            var categoryId = selectedCategory.Id;

            var spent = _context.Transactions
                        .Where(t => t.Type.ToLower() == "витрата"
                                 && t.CategoryId == categoryId
                                 && t.Date.Month == month
                                 && t.Date.Year == year)
                        .Sum(t => t.Amount);

            var limit = _context.BudgetLimits
                        .FirstOrDefault(l => l.CategoryId == categoryId
                            && l.Month == month
                            && l.Year == year
                        );

            if (limit != null && spent + transaction.Amount > limit.LimitAmount)
            {
                var result = MessageBox.Show(
                    $"⚠️ Ви перевищуєте бюджет по категорії «{selectedCategory.Name}». Продовжити?",
                    "Перевищення ліміту",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.No)
                    return false;
            }

            return true;
        }
    }
}