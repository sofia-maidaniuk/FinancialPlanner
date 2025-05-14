using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp_FinancialPlanner.ViewModels;

namespace WpfApp_FinancialPlanner.Views.balance
{
    public partial class BalancePage : Page
    {
        private AppDbContext _context => App.Services.GetRequiredService<AppDbContext>();
        private BalanceViewModel ViewModel => DataContext as BalanceViewModel;

        public BalancePage()
        {
            InitializeComponent();
            DataContext = new BalanceViewModel(_context);
        }

        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddBalanceWindow();
            if (window.ShowDialog() == true)
            {
                RefreshBalances();
            }
        }

        private void EditBalance_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedBalance(sender) is Balance selectedBalance)
            {
                var editWindow = new EditBalanceWindow(selectedBalance);
                if (editWindow.ShowDialog() == true)
                {
                    ApplyBalanceChanges(editWindow.EditedBalance);
                    RefreshBalances();
                }
            }
        }

        private void DeleteBalance_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedBalance(sender) is Balance selectedBalance &&
                ConfirmDeletion(selectedBalance.Name))
            {
                _context.Balances.Remove(selectedBalance);
                _context.SaveChanges();
                ViewModel.Balances.Remove(selectedBalance);
            }
        }

        private void RefreshBalances()
        {
            ViewModel.Balances.Clear();
            foreach (var balance in _context.Balances.ToList())
            {
                ViewModel.Balances.Add(balance);
            }
        }

        private Balance? GetSelectedBalance(object sender)
        {
            return (sender as Button)?.DataContext as Balance;
        }

        private void ApplyBalanceChanges(Balance edited)
        {
            var original = _context.Balances.FirstOrDefault(b => b.Id == edited.Id);
            if (original != null)
            {
                original.Name = edited.Name;
                original.Amount = edited.Amount;
                original.Icon = edited.Icon;
                _context.SaveChanges();
            }
        }

        private bool ConfirmDeletion(string name)
        {
            var result = MessageBox.Show(
                $"Ви дійсно хочете видалити баланс '{name}'?",
                "Підтвердження", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }
}