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

public partial class MainWindow : Window
{
    private readonly AnalyticsPage _analyticsPage;
    private readonly BalancePage _balancePage;
    private readonly CategoriesPage _categoriesPage;
    private readonly TransactionsPage _transactionsPage;
    private readonly BudgetLimitsPage _budgetPage;

    public MainWindow(
        AnalyticsPage analyticsPage,
        BalancePage balancePage,
        CategoriesPage categoriesPage,
        TransactionsPage transactionsPage,
        BudgetLimitsPage budgetPage)
    {
        InitializeComponent();

        _analyticsPage = analyticsPage;
        _balancePage = balancePage;
        _categoriesPage = categoriesPage;
        _transactionsPage = transactionsPage;
        _budgetPage = budgetPage;

        MainFrame.Navigate(_analyticsPage);
    }

    private void NavigateToBalance(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(_balancePage);

    private void NavigateToCategories(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(_categoriesPage);

    private void NavigateToTransactions(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(_transactionsPage);

    private void NavigateToAnalytics(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(_analyticsPage);

    private void NavigateToBudgetLimits(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(_budgetPage);
}
    