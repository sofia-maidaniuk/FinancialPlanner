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
        private AppDbContext _context => App.Services.GetRequiredService<AppDbContext>();
        private CategoryViewModel ViewModel => DataContext as CategoryViewModel;

        public CategoriesPage()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel(_context);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddCategoryWindow();
            if (addWindow.ShowDialog() == true)
            {
                AddCategoryToDb(addWindow.CreatedCategory);
                RefreshCategories();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedCategory(sender) is Category category &&
                ConfirmDeletion(category.Name))
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                RefreshCategories();
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedCategory(sender) is Category selectedCategory)
            {
                var editWindow = new EditCategoryWindow(selectedCategory);
                if (editWindow.ShowDialog() == true)
                {
                    UpdateCategoryInDb(editWindow.EditedCategory);
                    RefreshCategories();
                }
            }
        }

        private void RefreshCategories()
        {
            ViewModel?.LoadCategories();
        }

        private Category? GetSelectedCategory(object sender)
        {
            return (sender as Button)?.DataContext as Category;
        }

        private bool ConfirmDeletion(string name)
        {
            var result = MessageBox.Show(
                $"Видалити категорію '{name}'?",
                "Підтвердження", MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }

        private void AddCategoryToDb(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        private void UpdateCategoryInDb(Category updated)
        {
            var existing = _context.Categories.FirstOrDefault(c => c.Id == updated.Id);
            if (existing != null)
            {
                existing.Name = updated.Name;
                existing.Type = updated.Type;
                existing.Icon = updated.Icon;
                _context.SaveChanges();
            }
        }
    }
}