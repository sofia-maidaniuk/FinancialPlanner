using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_FinancialPlanner.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = "витрата"; // "витрата" або "дохід"
        
        [Required]
        public string Icon { get; set; } = string.Empty;

        public ICollection<Transaction>? Transactions { get; set; }

        public override string ToString()
        {
            return $"{Icon} {Name}";
        }

    }
}
