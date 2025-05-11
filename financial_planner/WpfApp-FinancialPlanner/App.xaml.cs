using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ClassLibrary_FinancialPlanner.Data;

namespace WpfApp_FinancialPlanner;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=finance.db"));


        Services = services.BuildServiceProvider();

        base.OnStartup(e);
    }
}

