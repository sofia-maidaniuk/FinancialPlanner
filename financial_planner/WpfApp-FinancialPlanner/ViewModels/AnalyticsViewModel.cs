﻿using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ClassLibrary_FinancialPlanner.Interfaces;
using ClassLibrary_FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary_FinancialPlanner.Data;
using ClassLibrary_FinancialPlanner.Services;

namespace WpfApp_FinancialPlanner.ViewModels
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        private readonly ITransactionRepository _repository;
        private readonly AppDbContext _context;

        private PlotModel _monthlyPlotModel;
        public PlotModel MonthlyPlotModel
        {
            get => _monthlyPlotModel;
            set { _monthlyPlotModel = value; OnPropertyChanged(nameof(MonthlyPlotModel)); }
        }

        private List<CategoryExpense> _expensesByCategory = new();
        public List<CategoryExpense> ExpensesByCategory
        {
            get => _expensesByCategory;
            set { _expensesByCategory = value; OnPropertyChanged(nameof(ExpensesByCategory)); }
        }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public AnalyticsViewModel(ITransactionRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
            DateFrom = DateTime.Now.AddMonths(-3);
            DateTo = DateTime.Now;
            GenerateMonthlyChart();
        }

        public async void GenerateMonthlyChart()
        {
            var transactions = await _repository.GetAllAsync();
            transactions = FilterTransactionsByDate(transactions);

            MonthlyPlotModel = CreateMonthlyPlot(transactions);

            var analyticsService = new AnalyticsService(_context);
            ExpensesByCategory = await analyticsService.GetExpensesByCategoryAsync(transactions, DateTo);
        }

        private List<Transaction> FilterTransactionsByDate(List<Transaction> transactions)
        {
            if (DateFrom.HasValue && DateTo.HasValue)
            {
                return transactions
                    .Where(t => t.Date.Date >= DateFrom.Value.Date && t.Date.Date <= DateTo.Value.Date)
                    .ToList();
            }
            return transactions;
        }

        private PlotModel CreateMonthlyPlot(List<Transaction> transactions)
        {
            var grouped = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    MonthText = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM", new CultureInfo("uk-UA")),
                    Income = g.Where(t => t.Type.ToLower() == "дохід").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type.ToLower() == "витрата").Sum(t => t.Amount)
                })
                .ToList();

            var model = new PlotModel();
            var incomeSeries = new BarSeries { Title = "Доходи", FillColor = OxyColors.SkyBlue };
            var expenseSeries = new BarSeries { Title = "Витрати", FillColor = OxyColors.CornflowerBlue };

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left, Angle = 90 };
            foreach (var g in grouped)
            {
                categoryAxis.Labels.Add(g.MonthText);
                incomeSeries.Items.Add(new BarItem { Value = (double)g.Income });
                expenseSeries.Items.Add(new BarItem { Value = (double)g.Expense });
            }

            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Title = "Сума ₴" });
            model.Series.Add(incomeSeries);
            model.Series.Add(expenseSeries);
            return model;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
