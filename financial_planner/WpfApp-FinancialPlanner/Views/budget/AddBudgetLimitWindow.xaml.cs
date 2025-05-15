using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary_FinancialPlanner.Data;

namespace WpfApp_FinancialPlanner.Views.budget
{
    public partial class AddBudgetLimitWindow : Window
    {
        private readonly AppDbContext _context;

        public BudgetLimit CreatedLimit { get; private set; } = new();

        public AddBudgetLimitWindow()
        {
            InitializeComponent();
            _context = App.Services.GetRequiredService<AppDbContext>();

            CategoryComboBox.ItemsSource = _context.Categories.ToList();
            CategoryComboBox.DisplayMemberPath = "Name";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is not Category category ||
                MonthComboBox.SelectedItem is not ComboBoxItem monthItem ||
                !int.TryParse(YearBox.Text, out int year) ||
                !decimal.TryParse(AmountBox.Text, out decimal amount))
            {
                MessageBox.Show("Заповніть усі поля коректно.");
                return;
            }

            int month = MonthComboBox.SelectedIndex + 1;

            CreatedLimit.CategoryId = category.Id;
            CreatedLimit.LimitAmount = amount;
            CreatedLimit.Year = year;
            CreatedLimit.Month = month;

            DialogResult = true;
            Close();
        }
    }
}
