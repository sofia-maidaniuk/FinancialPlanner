using System.Windows;
using System.Windows.Controls;
using ClassLibrary_FinancialPlanner.Models;
using ClassLibrary_FinancialPlanner.Data;
using Microsoft.Extensions.DependencyInjection;
using WpfApp_FinancialPlanner.ViewModels;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
            var context = App.Services.GetRequiredService<AppDbContext>();
            DataContext = new CategoryViewModel(context);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddCategoryWindow();
            if (addWindow.ShowDialog() == true)
            {
                var context = App.Services.GetRequiredService<AppDbContext>();
                context.Categories.Add(addWindow.CreatedCategory);
                context.SaveChanges();

                var viewModel = DataContext as CategoryViewModel;
                viewModel?.LoadCategories();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Category category)
            {
                var result = MessageBox.Show($"Видалити категорію '{category.Name}'?", "Підтвердження", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    var context = App.Services.GetRequiredService<AppDbContext>();
                    context.Categories.Remove(category);
                    context.SaveChanges();

                    var viewModel = DataContext as CategoryViewModel;
                    viewModel?.LoadCategories();
                }
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Category selectedCategory)
            {
                var editWindow = new EditCategoryWindow(selectedCategory);
                if (editWindow.ShowDialog() == true)
                {
                    var context = App.Services.GetRequiredService<AppDbContext>();
                    var existing = context.Categories.FirstOrDefault(c => c.Id == editWindow.EditedCategory.Id);
                    if (existing != null)
                    {
                        existing.Name = editWindow.EditedCategory.Name;
                        existing.Type = editWindow.EditedCategory.Type;
                        existing.Icon = editWindow.EditedCategory.Icon;
                        context.SaveChanges();

                        var viewModel = DataContext as CategoryViewModel;
                        viewModel?.LoadCategories();
                    }
                }
            }
        }
    }
}
