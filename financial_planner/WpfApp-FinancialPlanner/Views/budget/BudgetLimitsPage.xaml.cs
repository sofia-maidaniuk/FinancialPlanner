using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WpfApp_FinancialPlanner.ViewModels;
using ClassLibrary_FinancialPlanner.Models;
using WpfApp_FinancialPlanner.Views.budget;

namespace WpfApp_FinancialPlanner.Views.budget
{
    public partial class BudgetLimitsPage : Page
    {
        private BudgetLimitViewModel ViewModel => DataContext as BudgetLimitViewModel;

        public BudgetLimitsPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<BudgetLimitViewModel>();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddBudgetLimitWindow();
            if (addWindow.ShowDialog() == true)
            {
                ViewModel.AddLimitAsync(addWindow.CreatedLimit);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedLimit is BudgetLimit selected)
            {
                var editWindow = new EditBudgetLimitWindow(selected);
                if (editWindow.ShowDialog() == true)
                {
                    ViewModel.UpdateLimit(editWindow.EditedLimit);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedLimit is BudgetLimit selected &&
                MessageBox.Show($"Видалити ліміт для «{selected.Category.Name}»?",
                "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ViewModel.DeleteLimit(selected);
            }
        }
    }
}
