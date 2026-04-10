using System.Globalization;
using System.Windows;
using RussianMilkApp.Data;
using RussianMilkApp.Models;

namespace RussianMilkApp;

public partial class AddBatchWindow : Window
{
    private List<Product> _products = new();
    private List<Warehouse> _warehouses = new();

    public AddBatchWindow()
    {
        InitializeComponent();
        Loaded += AddBatchWindow_Loaded;
    }

    private void AddBatchWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            _products = DbHelper.GetProducts();
            _warehouses = DbHelper.GetWarehouses();

            ProductComboBox.ItemsSource = _products;
            WarehouseComboBox.ItemsSource = _warehouses;

            if (_products.Count > 0)
                ProductComboBox.SelectedIndex = 0;

            if (_warehouses.Count > 0)
                WarehouseComboBox.SelectedIndex = 0;

            ProductionDatePicker.SelectedDate = DateTime.Today;
            ExpirationDatePicker.SelectedDate = DateTime.Today.AddDays(7);
            QuantityTextBox.Text = "100";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить справочники продуктов и складов.\n\n" + ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Close();
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductComboBox.SelectedItem is not Product selectedProduct)
        {
            MessageBox.Show("Выбери продукт.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (WarehouseComboBox.SelectedItem is not Warehouse selectedWarehouse)
        {
            MessageBox.Show("Выбери склад.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ProductionDatePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Укажи дату производства.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ExpirationDatePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Укажи дату окончания срока годности.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (ExpirationDatePicker.SelectedDate.Value.Date < ProductionDatePicker.SelectedDate.Value.Date)
        {
            MessageBox.Show("Дата окончания срока годности не может быть раньше даты производства.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(QuantityTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity) || quantity <= 0)
        {
            MessageBox.Show("Количество должно быть положительным целым числом.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            DbHelper.AddBatch(
                selectedProduct.ProductId,
                selectedWarehouse.WarehouseId,
                ProductionDatePicker.SelectedDate.Value,
                ExpirationDatePicker.SelectedDate.Value,
                quantity);

            MessageBox.Show("Партия успешно добавлена.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось сохранить партию.\n\n" + ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
