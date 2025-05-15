using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp_FinancialPlanner.Views.budget
{
    public partial class EditBudgetLimitWindow : Window
    {
        public BudgetLimit EditedLimit { get; private set; }

        public EditBudgetLimitWindow(BudgetLimit limitToEdit)
        {
            InitializeComponent();

            var context = App.Services.GetRequiredService<AppDbContext>();
            var categories = context.Categories.ToList();

            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedItem = categories.FirstOrDefault(c => c.Id == limitToEdit.CategoryId);

            // Заповнення інших полів
            AmountBox.Text = limitToEdit.LimitAmount.ToString();
            YearBox.Text = limitToEdit.Year.ToString();
            MonthComboBox.SelectedIndex = limitToEdit.Month - 1;

            EditedLimit = new BudgetLimit
            {
                Id = limitToEdit.Id,
                CategoryId = limitToEdit.CategoryId,
                Category = limitToEdit.Category,
                LimitAmount = limitToEdit.LimitAmount,
                Year = limitToEdit.Year,
                Month = limitToEdit.Month
            };

            DataContext = EditedLimit;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountBox.Text, out decimal amount) || amount < 0)
            {
                MessageBox.Show("Введіть коректну суму ліміту.");
                return;
            }

            if (CategoryComboBox.SelectedItem is not Category category)
            {
                MessageBox.Show("Оберіть категорію.");
                return;
            }

            if (!int.TryParse(YearBox.Text, out int year) || year < 2000 || year > DateTime.Now.Year + 10)
            {
                MessageBox.Show("Введіть коректний рік.");
                return;
            }

            if (MonthComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Оберіть місяць.");
                return;
            }

            EditedLimit.CategoryId = category.Id;
            EditedLimit.Category = category;
            EditedLimit.LimitAmount = amount;
            EditedLimit.Year = year;
            EditedLimit.Month = MonthComboBox.SelectedIndex + 1;

            DialogResult = true;
            Close();
        }
    }
}