using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class AddBalanceWindow : Window
    {
        public AddBalanceWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(out decimal amount)) return;

            string icon = ParseIconFromComboBox();
            var balance = CreateBalance(NameBox.Text.Trim(), amount, icon);
            SaveBalanceToDatabase(balance);

            DialogResult = true;
            Close();
        }

        private bool ValidateFields(out decimal amount)
        {
            amount = 0;

            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(AmountBox.Text))
            {
                MessageBox.Show("Заповніть усі поля!");
                return false;
            }

            if (!decimal.TryParse(AmountBox.Text, out amount))
            {
                MessageBox.Show("Сума повинна бути числом.");
                return false;
            }

            return true;
        }

        private string ParseIconFromComboBox()
        {
            var selectedItem = IconComboBox.SelectedItem as ComboBoxItem;
            var fullText = selectedItem?.Content.ToString() ?? "";
            return fullText.Split(' ')[0]; // повертає тільки emoji
        }

        private Balance CreateBalance(string name, decimal amount, string icon)
        {
            return new Balance
            {
                Name = name,
                Amount = amount,
                Icon = icon
            };
        }

        private void SaveBalanceToDatabase(Balance balance)
        {
            var context = App.Services.GetRequiredService<AppDbContext>();
            context.Balances.Add(balance);
            context.SaveChanges();
        }
    }
}
