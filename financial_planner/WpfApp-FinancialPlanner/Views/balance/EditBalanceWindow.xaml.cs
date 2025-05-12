using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class EditBalanceWindow : Window
    {
        public Balance EditedBalance { get; private set; }

        public EditBalanceWindow(Balance balanceToEdit)
        {
            InitializeComponent();

            // Копіюємо поточні значення
            EditedBalance = CloneBalance(balanceToEdit);

            PopulateFormFields();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetAmount(out decimal amount)) return;

            string icon = ParseSelectedIcon();

            ApplyChanges(NameBox.Text.Trim(), amount, icon);

            DialogResult = true;
            Close();
        }

        private Balance CloneBalance(Balance original)
        {
            return new Balance
            {
                Id = original.Id,
                Name = original.Name,
                Amount = original.Amount,
                Icon = original.Icon
            };
        }

        private void PopulateFormFields()
        {
            NameBox.Text = EditedBalance.Name;
            AmountBox.Text = EditedBalance.Amount.ToString();

            foreach (ComboBoxItem item in IconComboBox.Items)
            {
                if (item.Content.ToString().StartsWith(EditedBalance.Icon))
                {
                    IconComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private bool TryGetAmount(out decimal amount)
        {
            amount = 0;

            if (!decimal.TryParse(AmountBox.Text, out amount))
            {
                MessageBox.Show("Сума повинна бути числом.");
                return false;
            }

            return true;
        }

        private string ParseSelectedIcon()
        {
            var selectedItem = IconComboBox.SelectedItem as ComboBoxItem;
            return selectedItem?.Content.ToString().Split(' ')[0] ?? "";
        }

        private void ApplyChanges(string name, decimal amount, string icon)
        {
            EditedBalance.Name = name;
            EditedBalance.Amount = amount;
            EditedBalance.Icon = icon;
        }
    }
}