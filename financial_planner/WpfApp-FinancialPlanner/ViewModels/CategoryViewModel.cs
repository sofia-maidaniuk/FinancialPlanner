using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public CategoryViewModel(AppDbContext context)
        {
            _context = context;
            LoadCategories();
        }

        public void LoadCategories()
        {
            Categories = new ObservableCollection<Category>(_context.Categories.ToList());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
