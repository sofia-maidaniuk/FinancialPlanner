using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_FinancialPlanner.Models
{
    public class Balance
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Icon { get; set; } // шлях до іконки

        public Balance(string name, decimal amount, string icon)
        {
            Name = name;
            Amount = amount;
            Icon = icon;
        }
    }
}
