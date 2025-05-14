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

        public ObservableCollection<CategoryGroup> GroupedCategories { get; set; } = new();

        public CategoryViewModel(AppDbContext context)
        {
            _context = context;
            LoadCategories();
        }

        public void LoadCategories()
        {
            var all = _context.Categories.ToList();

            var grouped = all
                .GroupBy(c => c.Type)
                .Select(g => new CategoryGroup
                {
                    Type = g.Key,
                    Categories = g.ToList()
                });

            GroupedCategories.Clear();
            foreach (var group in grouped)
                GroupedCategories.Add(group);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
