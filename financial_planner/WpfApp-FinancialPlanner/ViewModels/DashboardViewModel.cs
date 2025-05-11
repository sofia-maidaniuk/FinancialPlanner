using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class DashboardViewModel
    {
        public ObservableCollection<Balance> Balances { get; set; }

        public DashboardViewModel(AppDbContext context)
        {
            Balances = new ObservableCollection<Balance>(
                context.Balances.AsNoTracking().ToList()
            );
        }
    }
}
