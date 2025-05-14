using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary_FinancialPlanner.Models;
using ClassLibrary_FinancialPlanner.Interfaces;

namespace WpfApp_FinancialPlanner.Views.transaction
{
    public partial class AddTransactionWindow : Window
    {
        private readonly ITransactionRepository _repository;
        private readonly ClassLibrary_FinancialPlanner.Data.AppDbContext _context;

        public Transaction? CreatedTransaction { get; private set; }

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
                if (!decimal.TryParse(AmountBox.Text, out decimal amount))
                {
                    MessageBox.Show("Будь ласка, введіть коректну суму.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (BalanceComboBox.SelectedItem is not Balance selectedBalance)
                {
                    MessageBox.Show("Будь ласка, оберіть баланс.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (CategoryComboBox.SelectedItem is not Category selectedCategory)
                {
                    MessageBox.Show("Будь ласка, оберіть категорію.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
                {
                    MessageBox.Show("Будь ласка, введіть опис транзакції.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "витрата";

                var newTransaction = new Transaction
                {
                    Description = DescriptionBox.Text.Trim(),
                    Amount = amount,
                    BalanceId = selectedBalance.Id,
                    CategoryId = selectedCategory.Id,
                    Type = selectedType,
                    Date = DatePicker.SelectedDate ?? DateTime.Now
                };

                await _repository.AddAsync(newTransaction);
                CreatedTransaction = newTransaction;

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
    }
}