using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Balance> _balances;

        public ObservableCollection<Balance> Balances
        {
            get => _balances;
            set
            {
                _balances = value;
                OnPropertyChanged(nameof(Balances));
            }
        }

        public DashboardViewModel(AppDbContext context)
        {
            _context = context;
            LoadBalances();
        }

        public void LoadBalances()
        {
            var list = _context.Balances.AsNoTracking().ToList();
            Balances = new ObservableCollection<Balance>(list); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
