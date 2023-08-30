using System.Data.SQLite;

namespace ShopPC
{
    public static class DatabaseManager
    {
        public static void CreateDatabaseIfNotExists(string connectionString)
        {
            if (!DatabaseExists(connectionString))
            {
                SQLiteConnection.CreateFile("ShopPC.db");
                CreateTables(connectionString);
            }
        }



        private static bool DatabaseExists(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static void CreateTables(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createCategoriesTableQuery = "CREATE TABLE Categories (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT UNIQUE, Description TEXT)";
                using (SQLiteCommand command = new SQLiteCommand(createCategoriesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createSuppliersTableQuery = "CREATE TABLE Suppliers (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Name TEXT, Phone TEXT, Address TEXT)";
                using (SQLiteCommand command = new SQLiteCommand(createSuppliersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createUsersTableQuery = "CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Role TEXT)";
                using (SQLiteCommand command = new SQLiteCommand(createUsersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createProductsTableQuery = "CREATE TABLE Products (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Characteristic TEXT, Quantity INTEGER, Cost REAL, CategoryId INTEGER, SupplierId INTEGER, FOREIGN KEY (CategoryId) REFERENCES Categories(Id), FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id))";
                using (SQLiteCommand command = new SQLiteCommand(createProductsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createSalesTableQuery = "CREATE TABLE Sales (Id INTEGER PRIMARY KEY AUTOINCREMENT, ProductId INTEGER, Quantity INTEGER, TotalCost REAL, SellerId INTEGER, SaleDate DATETIME, FOREIGN KEY (ProductId) REFERENCES Products(Id), FOREIGN KEY (SellerId) REFERENCES Users(Id))";
                using (SQLiteCommand command = new SQLiteCommand(createSalesTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Добавьте создание других таблиц, если необходимо

                // Закрываем соединение
                connection.Close();
            }
        }
    }
}
