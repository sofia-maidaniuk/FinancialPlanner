using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp_FinancialPlanner.ViewModels;

namespace WpfApp_FinancialPlanner.Views.analytics
{
    public partial class AnalyticsPage : Page
    {
        public AnalyticsPage()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<AnalyticsViewModel>();
        }

        private void RefreshChart_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AnalyticsViewModel viewModel)
            {
                viewModel.GenerateMonthlyChart(); 
            }
        }
    }
}
