using ClassLibrary_FinancialPlanner.Models;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp_FinancialPlanner.Views
{
    public partial class EditCategoryWindow : Window
    {
        public Category EditedCategory { get; private set; }

        public EditCategoryWindow(Category categoryToEdit)
        {
            InitializeComponent();

            EditedCategory = new Category
            {
                Id = categoryToEdit.Id,
                Name = categoryToEdit.Name,
                Type = categoryToEdit.Type,
                Icon = categoryToEdit.Icon
            };

            NameBox.Text = EditedCategory.Name;

            foreach (ComboBoxItem item in TypeComboBox.Items)
            {
                if (item.Content.ToString() == EditedCategory.Type)
                {
                    TypeComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in IconComboBox.Items)
            {
                if (item.Content.ToString().StartsWith(EditedCategory.Icon))
                {
                    IconComboBox.SelectedItem = item;
                    break;
                }
            }
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

            EditedCategory.Name = NameBox.Text;
            EditedCategory.Type = selectedType.Content.ToString();
            EditedCategory.Icon = selectedIcon.Content.ToString().Split(' ')[0];

            DialogResult = true;
            Close();
        }
    }
}
