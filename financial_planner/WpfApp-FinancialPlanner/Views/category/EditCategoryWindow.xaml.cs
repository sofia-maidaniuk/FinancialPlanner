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
            InitializeEditedCategory(categoryToEdit);
            FillFields();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            EditedCategory.Name = NameBox.Text.Trim();
            EditedCategory.Type = GetSelectedType();
            EditedCategory.Icon = GetSelectedIcon();

            DialogResult = true;
            Close();
        }

        private void InitializeEditedCategory(Category source)
        {
            EditedCategory = new Category
            {
                Id = source.Id,
                Name = source.Name,
                Type = source.Type,
                Icon = source.Icon
            };
        }

        private void FillFields()
        {
            NameBox.Text = EditedCategory.Name;

            SelectComboBoxItem(TypeComboBox, EditedCategory.Type);
            SelectComboBoxItemStartsWith(IconComboBox, EditedCategory.Icon);
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SelectComboBoxItemStartsWith(ComboBox comboBox, string startsWith)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString().StartsWith(startsWith))
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
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
    }
}