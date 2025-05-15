using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WpfApp_FinancialPlanner.ViewModels;
using ClassLibrary_FinancialPlanner.Models;

namespace WpfApp_FinancialPlanner.Views.transaction
{
    public partial class TransactionsPage : Page
    {
        private TransactionsViewModel _viewModel;

        public TransactionsPage()
        {
            InitializeComponent();
            var repository = App.Services.GetRequiredService<ITransactionRepository>();
            var context = App.Services.GetRequiredService<AppDbContext>();
            _viewModel = new TransactionsViewModel(repository, context);
            DataContext = _viewModel;

            Loaded += async (s, e) => await _viewModel.LoadAsync();
        }


        private async void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddTransactionWindow();
            window.TransactionAdded += async (transaction) =>
            {
                await _viewModel.LoadAsync();
            };

            window.ShowDialog();
        }

        private async void EditTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Transaction transactionToEdit)
            {
                var editWindow = new EditTransactionWindow();
                editWindow.SetTransaction(transactionToEdit);

                if (editWindow.ShowDialog() == true)
                {
                    var repository = App.Services.GetRequiredService<ITransactionRepository>();
                    await repository.UpdateAsync(editWindow.EditedTransaction);

                    if (DataContext is TransactionsViewModel viewModel)
                    {
                        await viewModel.LoadAsync();
                    }
                }
            }
        }

        private async void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Transaction transactionToDelete)
            {
                var result = MessageBox.Show(
                    $"Видалити транзакцію: \"{transactionToDelete.Description}\" на {transactionToDelete.Amount:N2} ₴?",
                    "Підтвердіть видалення",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _viewModel.DeleteAndReloadAsync(transactionToDelete.Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка: {ex.Message}", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is TransactionsViewModel vm)
            {
                await vm.LoadAsync();
            }
        }

        private async void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is TransactionsViewModel vm)
            {
                vm.SearchText = "";
                vm.SelectedType = "усі";
                vm.SelectedCategory = "усі";
                vm.DateFrom = null;
                vm.DateTo = null;

                // Оновлення фільтрів і перегляд
                await vm.LoadAsync();
            }
        }
    }
}
