using System.Windows;
using RussianMilkApp.Data;

namespace RussianMilkApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadBatches();
    }

    private void LoadBatches(string? filter = null)
    {
        try
        {
            var batches = DbHelper.GetBatches(filter);
            BatchesGrid.ItemsSource = batches;
            StatusTextBlock.Text = $"Загружено записей: {batches.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось загрузить данные. Проверь строку подключения и наличие базы данных.\n\n" + ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            StatusTextBlock.Text = "Ошибка подключения к базе данных";
        }
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        LoadBatches(SearchTextBox.Text.Trim());
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Clear();
        LoadBatches();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadBatches(SearchTextBox.Text.Trim());
    }

    private void AddBatchButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var window = new AddBatchWindow
            {
                Owner = this
            };

            var result = window.ShowDialog();
            if (result == true)
            {
                LoadBatches(SearchTextBox.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Не удалось открыть окно добавления партии.\n\n" + ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
