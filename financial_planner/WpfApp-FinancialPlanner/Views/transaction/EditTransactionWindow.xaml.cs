using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary_FinancialPlanner.Models;
using ClassLibrary_FinancialPlanner.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary_FinancialPlanner.Data;

namespace WpfApp_FinancialPlanner.Views.transaction
{
    public partial class EditTransactionWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly ITransactionRepository _repository;

        private Transaction _originalTransaction;

        public Transaction EditedTransaction { get; private set; }

        public EditTransactionWindow()
        {
            InitializeComponent();

            _context = App.Services.GetRequiredService<AppDbContext>();
            _repository = App.Services.GetRequiredService<ITransactionRepository>();

            BalanceComboBox.ItemsSource = _context.Balances.ToList();
        }

        public void SetTransaction(Transaction transaction)
        {
            _originalTransaction = transaction;

            // Заповнюємо поля
            DescriptionBox.Text = transaction.Description;
            AmountBox.Text = transaction.Amount.ToString("0.00");
            DatePicker.SelectedDate = transaction.Date;
            BalanceComboBox.SelectedItem = _context.Balances.FirstOrDefault(b => b.Id == transaction.BalanceId);

            // Вибір типу
            foreach (ComboBoxItem item in TypeComboBox.Items)
            {
                if ((string)item.Content == transaction.Type)
                {
                    TypeComboBox.SelectedItem = item;
                    break;
                }
            }

            // Завантажити категорії за типом
            LoadCategories(transaction.Type);
            CategoryComboBox.SelectedItem = _context.Categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            LoadCategories(selectedType);
        }

        private void LoadCategories(string type)
        {
            CategoryComboBox.ItemsSource = _context.Categories
                .Where(c => c.Type == type)
                .ToList();

            CategoryComboBox.SelectedIndex = 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountBox.Text, out decimal amount))
            {
                MessageBox.Show("Сума повинна бути числом", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Опис не може бути порожнім", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BalanceComboBox.SelectedItem is not Balance selectedBalance ||
                CategoryComboBox.SelectedItem is not Category selectedCategory)
            {
                MessageBox.Show("Оберіть баланс і категорію", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "витрата";

            EditedTransaction = new Transaction
            {
                Id = _originalTransaction.Id, 
                Description = DescriptionBox.Text.Trim(),
                Amount = amount,
                Date = DatePicker.SelectedDate ?? DateTime.Now,
                BalanceId = selectedBalance.Id,
                CategoryId = selectedCategory.Id,
                Type = selectedType
            };

            DialogResult = true;
            Close();
        }
    }
}
