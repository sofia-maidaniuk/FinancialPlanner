using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class EditBalanceWindow : Window
    {
        public Balance EditedBalance { get; private set; }

        public EditBalanceWindow(Balance balanceToEdit)
        {
            InitializeComponent();

            // Копіюємо поточні значення
            EditedBalance = new Balance
            {
                Id = balanceToEdit.Id,
                Name = balanceToEdit.Name,
                Amount = balanceToEdit.Amount,
                Icon = balanceToEdit.Icon
            };

            NameBox.Text = EditedBalance.Name;
            AmountBox.Text = EditedBalance.Amount.ToString();

            // Встановити вибрану іконку
            foreach (ComboBoxItem item in IconComboBox.Items)
            {
                if (item.Content.ToString().StartsWith(EditedBalance.Icon))
                {
                    IconComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountBox.Text, out decimal amount))
            {
                MessageBox.Show("Сума повинна бути числом.");
                return;
            }

            var selectedItem = IconComboBox.SelectedItem as ComboBoxItem;
            var icon = selectedItem?.Content.ToString().Split(' ')[0] ?? "";

            EditedBalance.Name = NameBox.Text;
            EditedBalance.Amount = amount;
            EditedBalance.Icon = icon;

            DialogResult = true;
            Close();
        }
    }
}
