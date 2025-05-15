using ClassLibrary_FinancialPlanner.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp_FinancialPlanner.Views;
using WpfApp_FinancialPlanner.Views.transaction;
using WpfApp_FinancialPlanner.Views.balance;
using WpfApp_FinancialPlanner.Views.analytics;
using WpfApp_FinancialPlanner.Views.budget; 

namespace WpfApp_FinancialPlanner;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new AnalyticsPage());
    }

    private void NavigateToBalance(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new BalancePage());
    }

    private void NavigateToCategories(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new CategoriesPage());
    }
    private void NavigateToTransactions(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TransactionsPage());
    }

    private void NavigateToAnalytics(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new AnalyticsPage());
    }

    private void NavigateToBudgetLimits(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new BudgetLimitsPage());
    }
}