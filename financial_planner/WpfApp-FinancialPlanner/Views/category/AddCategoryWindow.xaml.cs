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
            if (!ValidateInput()) return;

            string name = NameBox.Text.Trim();
            string icon = GetSelectedIcon();
            string type = GetSelectedType();

            AssignCategoryData(name, icon, type);

            DialogResult = true;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Назва не може бути порожньою.");
                return false;
            }

            if (IconComboBox.SelectedItem is not ComboBoxItem || TypeComboBox.SelectedItem is not ComboBoxItem)
            {
                MessageBox.Show("Виберіть тип та іконку.");
                return false;
            }

            return true;
        }

        private string GetSelectedIcon()
        {
            var item = IconComboBox.SelectedItem as ComboBoxItem;
            return item?.Content.ToString()?.Split(' ')[0] ?? "";
        }

        private string GetSelectedType()
        {
            var item = TypeComboBox.SelectedItem as ComboBoxItem;
            return item?.Content.ToString() ?? "витрата";
        }

        private void AssignCategoryData(string name, string icon, string type)
        {
            CreatedCategory.Name = name;
            CreatedCategory.Icon = icon;
            CreatedCategory.Type = type;
        }
    }
}