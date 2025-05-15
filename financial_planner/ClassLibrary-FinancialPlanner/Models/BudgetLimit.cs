using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary_FinancialPlanner.Models
{
    public class BudgetLimit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        public decimal LimitAmount { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        [NotMapped]
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM", new System.Globalization.CultureInfo("uk-UA"));
    }
}
