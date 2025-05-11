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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp_FinancialPlanner.ViewModels;

namespace WpfApp_FinancialPlanner.Views
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            var context = App.Services.GetRequiredService<AppDbContext>();
            DataContext = new DashboardViewModel(context);
        }

        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddBalanceWindow();
            if (window.ShowDialog() == true)
            {
                var context = App.Services.GetRequiredService<AppDbContext>();
                var viewModel = DataContext as DashboardViewModel;

                viewModel.Balances.Clear();
                foreach (var b in context.Balances.ToList())
                {
                    viewModel.Balances.Add(b);
                }
            }
        }

        private void EditBalance_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Balance selectedBalance)
            {
                var editWindow = new EditBalanceWindow(selectedBalance);
                if (editWindow.ShowDialog() == true)
                {
                    var context = App.Services.GetRequiredService<AppDbContext>();

                    var original = context.Balances.FirstOrDefault(b => b.Id == editWindow.EditedBalance.Id);
                    if (original != null)
                    {
                        original.Name = editWindow.EditedBalance.Name;
                        original.Amount = editWindow.EditedBalance.Amount;
                        original.Icon = editWindow.EditedBalance.Icon;

                        context.SaveChanges();

                        // Оновити ViewModel
                        var viewModel = DataContext as DashboardViewModel;
                        viewModel.LoadBalances();
                    }
                }
            }
        }


        private void DeleteBalance_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Balance selectedBalance)
            {
                var result = MessageBox.Show($"Ви дійсно хочете видалити баланс '{selectedBalance.Name}'?",
                                             "Підтвердження", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var context = App.Services.GetRequiredService<AppDbContext>();
                    context.Balances.Remove(selectedBalance);
                    context.SaveChanges();

                    // Оновлення ViewModel
                    var viewModel = DataContext as DashboardViewModel;
                    viewModel.Balances.Remove(selectedBalance);
                }
            }
        }
    }
}
