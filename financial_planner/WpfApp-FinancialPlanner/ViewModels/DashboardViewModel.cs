using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_FinancialPlanner.Models;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class DashboardViewModel
    {
        public ObservableCollection<Balance> Balances { get; set; }

        public DashboardViewModel()
        {
            Balances = new ObservableCollection<Balance>();
        }
    }
}
