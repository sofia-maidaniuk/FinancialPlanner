using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Repositories;
using WpfApp_FinancialPlanner.ViewModels;
namespace WpfApp_FinancialPlanner;
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FinanceDb;Trusted_Connection=True;"));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IBudgetLimitRepository, BudgetLimitRepository>();

        services.AddTransient<AnalyticsViewModel>();
        services.AddScoped<BudgetLimitViewModel>();
        services.AddScoped<TransactionsViewModel>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddTransient<AnalyticsPage>();
        services.AddTransient<BalancePage>();
        services.AddTransient<CategoriesPage>();
        services.AddTransient<TransactionsPage>();
        services.AddTransient<BudgetLimitsPage>();
        Services = services.BuildServiceProvider();

        using (var scope = Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            SeedData.Initialize(context);
        }

        base.OnStartup(e);
    }
}
