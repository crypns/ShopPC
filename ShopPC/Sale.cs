using System;
using System.Data.SQLite;

namespace ShopPC
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public int SellerId { get; set; }
        public DateTime SaleDate { get; set; }

        // Конструктор по умолчанию
        public Sale()
        {
        }

        // Конструктор с параметрами
        public Sale(int productId, int quantity, decimal totalCost, int sellerId, DateTime saleDate)
        {
            ProductId = productId;
            Quantity = quantity;
            TotalCost = totalCost;
            SellerId = sellerId;
            SaleDate = saleDate;
        }

        // Метод для добавления записи о продаже в базу данных
        public void AddToDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Sales (ProductId, Quantity, TotalCost, SellerId, SaleDate) VALUES (@ProductId, @Quantity, @TotalCost, @SellerId, @SaleDate)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@Quantity", Quantity);
                    command.Parameters.AddWithValue("@TotalCost", TotalCost);
                    command.Parameters.AddWithValue("@SellerId", SellerId);
                    command.Parameters.AddWithValue("@SaleDate", SaleDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Другие методы, если необходимо
    }
}
