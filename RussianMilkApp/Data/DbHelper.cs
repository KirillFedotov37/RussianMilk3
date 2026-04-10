using Microsoft.Data.SqlClient;
using RussianMilkApp.Models;

namespace RussianMilkApp.Data;

public static class DbHelper
{
    
    public static string ConnectionString { get; set; } =
        @"Server=.\SQLEXPRESS;Database=РосМолоко;Trusted_Connection=True;TrustServerCertificate=True;";

    public static List<BatchView> GetBatches(string? productFilter = null)
    {
        var result = new List<BatchView>();

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var query = @"
            SELECT
                п.[IdПартии],
                пр.[Название],
                с.[НазваниеСклада],
                п.[ДатаПроизводства],
                п.[СрокГодности],
                п.[Количество]
            FROM [Партии] п
            INNER JOIN [Продукция] пр ON пр.[IdПродукта] = п.[IdПродукта]
            INNER JOIN [Склады] с ON с.[IdСклада] = п.[IdСклада]
            WHERE (@filter IS NULL OR @filter = '' OR пр.[Название] LIKE N'%' + @filter + N'%')
            ORDER BY п.[IdПартии] DESC;";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@filter", (object?)productFilter ?? DBNull.Value);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new BatchView
            {
                BatchId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                WarehouseName = reader.GetString(2),
                ProductionDate = reader.GetDateTime(3),
                ExpirationDate = reader.GetDateTime(4),
                Quantity = reader.GetInt32(5)
            });
        }

        return result;
    }

    public static List<Product> GetProducts()
    {
        var result = new List<Product>();

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        const string query = @"
            SELECT [IdПродукта], [Название], [Жирность], [ТипУпаковки]
            FROM [Продукция]
            ORDER BY [Название];";

        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                FatContent = reader.GetDecimal(2),
                PackageType = reader.GetString(3)
            });
        }

        return result;
    }

    public static List<Warehouse> GetWarehouses()
    {
        var result = new List<Warehouse>();

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        const string query = @"
            SELECT [IdСклада], [НазваниеСклада], [Адрес]
            FROM [Склады]
            ORDER BY [НазваниеСклада];";

        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new Warehouse
            {
                WarehouseId = reader.GetInt32(0),
                WarehouseName = reader.GetString(1),
                Address = reader.GetString(2)
            });
        }

        return result;
    }

    public static void AddBatch(int productId, int warehouseId, DateTime productionDate, DateTime expirationDate, int quantity)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        const string query = @"
            INSERT INTO [Партии] ([IdПродукта], [IdСклада], [ДатаПроизводства], [СрокГодности], [Количество])
            VALUES (@productId, @warehouseId, @productionDate, @expirationDate, @quantity);";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@productId", productId);
        command.Parameters.AddWithValue("@warehouseId", warehouseId);
        command.Parameters.AddWithValue("@productionDate", productionDate.Date);
        command.Parameters.AddWithValue("@expirationDate", expirationDate.Date);
        command.Parameters.AddWithValue("@quantity", quantity);

        command.ExecuteNonQuery();
    }
}
