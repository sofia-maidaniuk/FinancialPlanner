using ClassLibrary_FinancialPlanner.Models;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class AddCategoryWindow : Window
    {
        public Category CreatedCategory { get; private set; } = new();

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Назва не може бути порожньою.");
                return;
            }

            var selectedIcon = IconComboBox.SelectedItem as ComboBoxItem;
            var selectedType = TypeComboBox.SelectedItem as ComboBoxItem;

            if (selectedIcon == null || selectedType == null)
            {
                MessageBox.Show("Виберіть тип та іконку.");
                return;
            }

            CreatedCategory.Name = NameBox.Text;
            CreatedCategory.Type = selectedType.Content.ToString();
            CreatedCategory.Icon = selectedIcon.Content.ToString().Split(' ')[0];

            DialogResult = true;
            Close();
        }
    }
}
