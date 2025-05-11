using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_FinancialPlanner.Models;

namespace Wpf_FinancialPlanner.ViewModels
{
    public class DashboardViewModel
    {
        public string Greeting => "Привіт, Олено!";
        public ObservableCollection<Balance> Balances { get; set; }

        public DashboardViewModel()
        {
            Balances = new ObservableCollection<Balance>
            {
                new Balance("Карта Visa", 12300m, "💳"),
                new Balance("Готівка", 2000m, "💵"),
                new Balance("Депозит", 50000m, "🏦")
            };
        }
    }
}
