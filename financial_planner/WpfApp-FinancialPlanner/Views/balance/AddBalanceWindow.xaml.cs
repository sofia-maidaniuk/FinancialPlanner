using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class AddBalanceWindow : Window
    {
        public AddBalanceWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(AmountBox.Text))
            {
                MessageBox.Show("Заповніть усі поля!");
                return;
            }

            if (!decimal.TryParse(AmountBox.Text, out decimal amount))
            {
                MessageBox.Show("Сума повинна бути числом.");
                return;
            }

            var context = App.Services.GetRequiredService<AppDbContext>();

            // Отримуємо вибраний ComboBoxItem
            var selectedItem = IconComboBox.SelectedItem as ComboBoxItem;
            var fullText = selectedItem?.Content.ToString() ?? "";
            var icon = fullText.Split(' ')[0]; // тільки emoji, без опису

            var balance = new Balance
            {
                Name = NameBox.Text,
                Amount = amount,
                Icon = icon
            };


            context.Balances.Add(balance);
            context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}
