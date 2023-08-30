    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;

    namespace ShopPC
    {
        public class Product
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Characteristic { get; set; }
            public int Quantity { get; set; }
            public decimal Cost { get; set; }
            public string CategoryName { get; set; }
            public string SupplierName { get; set; }

            // Конструктор по умолчанию
            public Product()
            {
            }

            // Конструктор с параметрами
            public Product(string title, string characteristic, int quantity, decimal cost, string categoryName, string supplierName)
            {
                Title = title;
                Characteristic = characteristic;
                Quantity = quantity;
                Cost = cost;
                CategoryName = categoryName;
                SupplierName = supplierName;
            }

            // Метод для добавления товара в базу данных
            public void AddToDatabase(string connectionString)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Получаем ID категории по её имени
                    int categoryId = GetCategoryIdByName(CategoryName, connection);

                    // Получаем ID поставщика по его имени
                    int supplierId = GetSupplierIdByName(SupplierName, connection);

                    string insertQuery = "INSERT INTO Products (Title, Characteristic, Quantity, Cost, CategoryId, SupplierId) VALUES (@Title, @Characteristic, @Quantity, @Cost, @CategoryId, @SupplierId)";
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Characteristic", Characteristic);
                        command.Parameters.AddWithValue("@Quantity", Quantity);
                        command.Parameters.AddWithValue("@Cost", Cost);
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@SupplierId", supplierId);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Метод для получения ID категории по её имени
            private int GetCategoryIdByName(string categoryName, SQLiteConnection connection)
            {
                string query = "SELECT Id FROM Categories WHERE Name = @Name";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", CategoryName);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }

            // Метод для получения ID поставщика по его имени
            private int GetSupplierIdByName(string supplierName, SQLiteConnection connection)
            {
                string query = "SELECT Id FROM Suppliers WHERE Title = @Title";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", SupplierName);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }

            public void UpdateInDatabase(string connectionString)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Получаем ID категории по её имени
                    int categoryId = GetCategoryIdByName(CategoryName, connection);

                    // Получаем ID поставщика по его имени
                    int supplierId = GetSupplierIdByName(SupplierName, connection);

                    string query = "UPDATE Products SET Title = @Title, Characteristic = @Characteristic, Quantity = @Quantity, Cost = @Cost, CategoryId = @CategoryId, SupplierId = @SupplierId WHERE Id = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Characteristic", Characteristic);
                        command.Parameters.AddWithValue("@Quantity", Quantity);
                        command.Parameters.AddWithValue("@Cost", Cost);
                        command.Parameters.AddWithValue("@CategoryId", categoryId);
                        command.Parameters.AddWithValue("@SupplierId", supplierId);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Метод для удаления товара из базы данных
            public void DeleteFromDatabase(string connectionString)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Products WHERE Id = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Метод для поиска товаров по названию или категории
            public static List<Product> SearchProducts(string searchQuery, decimal minPrice, decimal maxPrice, string connectionString)
            {
                List<Product> searchResults = new List<Product>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Title, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                                   "WHERE Title LIKE @SearchQuery AND Cost BETWEEN @MinPrice AND @MaxPrice";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                        command.Parameters.AddWithValue("@MinPrice", minPrice);
                        command.Parameters.AddWithValue("@MaxPrice", maxPrice);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal cost = Convert.ToDecimal(reader["Cost"]);
                                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                                int supplierId = Convert.ToInt32(reader["SupplierId"]);

                                string categoryName = GetCategoryNameById(categoryId, connection);
                                string supplierName = GetSupplierNameById(supplierId, connection);

                                searchResults.Add(new Product
                                {
                                    Id = id,
                                    Title = title,
                                    Quantity = quantity,
                                    Cost = cost,
                                    CategoryName = categoryName,
                                    SupplierName = supplierName
                                });
                            }
                        }
                    }
                }

                return searchResults;
            }

            public static List<Product> SearchProductsByCategory(string categoryName, string connectionString)
            {
                List<Product> searchResults = new List<Product>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Title, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                                   "WHERE CategoryId IN (SELECT Id FROM Categories WHERE Name = @CategoryName)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", categoryName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal cost = Convert.ToDecimal(reader["Cost"]);
                                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                                int supplierId = Convert.ToInt32(reader["SupplierId"]);

                                string supplierName = GetSupplierNameById(supplierId, connection);

                                searchResults.Add(new Product
                                {
                                    Id = id,
                                    Title = title,
                                    Quantity = quantity,
                                    Cost = cost,
                                    CategoryName = categoryName,
                                    SupplierName = supplierName
                                });
                            }
                        }
                    }
                }

                return searchResults;
            }
            public static List<Product> SearchProductsByCategoryAndPriceRange(string categoryName, decimal minPrice, decimal maxPrice, string connectionString)
            {
                List<Product> searchResults = new List<Product>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Title, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                                   "WHERE CategoryId IN (SELECT Id FROM Categories WHERE Name = @CategoryName) " +
                                   "AND Cost >= @MinPrice AND Cost <= @MaxPrice";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        command.Parameters.AddWithValue("@MinPrice", minPrice);
                        command.Parameters.AddWithValue("@MaxPrice", maxPrice);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal cost = Convert.ToDecimal(reader["Cost"]);
                                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                                int supplierId = Convert.ToInt32(reader["SupplierId"]);

                                string supplierName = GetSupplierNameById(supplierId, connection);

                                searchResults.Add(new Product
                                {
                                    Id = id,
                                    Title = title,
                                    Quantity = quantity,
                                    Cost = cost,
                                    CategoryName = categoryName,
                                    SupplierName = supplierName
                                });
                            }
                        }
                    }
                }

                return searchResults;
            }


            public static List<Product> SearchProductsByPriceRange(decimal minPrice, decimal maxPrice, string connectionString)
            {
                List<Product> searchResults = new List<Product>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Title, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                                   "WHERE Cost BETWEEN @MinPrice AND @MaxPrice";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MinPrice", minPrice);
                        command.Parameters.AddWithValue("@MaxPrice", maxPrice);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal cost = Convert.ToDecimal(reader["Cost"]);
                                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                                int supplierId = Convert.ToInt32(reader["SupplierId"]);

                                string categoryName = GetCategoryNameById(categoryId, connection);
                                string supplierName = GetSupplierNameById(supplierId, connection);

                                searchResults.Add(new Product
                                {
                                    Id = id,
                                    Title = title,
                                    Quantity = quantity,
                                    Cost = cost,
                                    CategoryName = categoryName,
                                    SupplierName = supplierName
                                });
                            }
                        }
                    }
                }

                return searchResults;
            }


            public static List<Product> SearchProducts(string searchQuery, string connectionString)
            {
                List<Product> searchResults = new List<Product>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Title, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                                   "WHERE Title LIKE @SearchQuery";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string title = reader.GetString(1);
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal cost = Convert.ToDecimal(reader["Cost"]);
                                int categoryId = Convert.ToInt32(reader["CategoryId"]);
                                int supplierId = Convert.ToInt32(reader["SupplierId"]);

                                string categoryName = GetCategoryNameById(categoryId, connection);
                                string supplierName = GetSupplierNameById(supplierId, connection);

                                searchResults.Add(new Product
                                {
                                    Id = id,
                                    Title = title,
                                    Quantity = quantity,
                                    Cost = cost,
                                    CategoryName = categoryName,
                                    SupplierName = supplierName
                                });
                            }
                        }
                    }
                }

                return searchResults;
            }

        public static List<Product> SearchProductsByCharacteristic(string characteristicSearchQuery, string connectionString)
        {
            List<Product> searchResults = new List<Product>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Title, Characteristic, Quantity, Cost, CategoryId, SupplierId FROM Products " +
                               "WHERE Characteristic LIKE @SearchQuery";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + characteristicSearchQuery + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string title = reader.GetString(1);
                            string characteristic = reader.GetString(2);
                            int quantity = Convert.ToInt32(reader["Quantity"]);
                            decimal cost = Convert.ToDecimal(reader["Cost"]);
                            int categoryId = Convert.ToInt32(reader["CategoryId"]);
                            int supplierId = Convert.ToInt32(reader["SupplierId"]);

                            string categoryName = GetCategoryNameById(categoryId, connection);
                            string supplierName = GetSupplierNameById(supplierId, connection);

                            searchResults.Add(new Product
                            {
                                Id = id,
                                Title = title,
                                Characteristic = characteristic,
                                Quantity = quantity,
                                Cost = cost,
                                CategoryName = categoryName,
                                SupplierName = supplierName
                            });
                        }
                    }
                }
            }

            return searchResults;
        }



        public static Product GetProductById(int productId, string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Title, Characteristic, Quantity, Cost, CategoryId, SupplierId FROM Products WHERE Id = @ProductId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string title = reader.GetString(1);
                            string characteristic = reader.GetString(2);
                            int quantity = Convert.ToInt32(reader["Quantity"]);
                            decimal cost = Convert.ToDecimal(reader["Cost"]);
                            int categoryId = Convert.ToInt32(reader["CategoryId"]);
                            int supplierId = Convert.ToInt32(reader["SupplierId"]);

                            string categoryName = GetCategoryNameById(categoryId, connection);
                            string supplierName = GetSupplierNameById(supplierId, connection);

                            return new Product
                            {
                                Id = productId,
                                Title = title,
                                Characteristic = characteristic,
                                Quantity = quantity,
                                Cost = cost,
                                CategoryName = categoryName,
                                SupplierName = supplierName
                            };
                        }
                    }
                }
            }

            return null; // Если товар не найден
        }

        public void UpdateProductQuantity(string connectionString, int productId, int newQuantity)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Products SET Quantity = @NewQuantity WHERE Id = @ProductId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                    command.ExecuteNonQuery();
                }
            }
        }


        // Метод для получения названия категории по её ID
        public static string GetCategoryNameById(int categoryId, SQLiteConnection connection)
            {
                string query = "SELECT Name FROM Categories WHERE Id = @CategoryId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    object result = command.ExecuteScalar();
                    return result != null ? result.ToString() : string.Empty;
                }
            }

            // Метод для получения названия поставщика по его ID
            public static string GetSupplierNameById(int supplierId, SQLiteConnection connection)
            {
                string query = "SELECT Title FROM Suppliers WHERE Id = @SupplierId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SupplierId", supplierId);
                    object result = command.ExecuteScalar();
                    return result != null ? result.ToString() : string.Empty;
                }
            }

            // Другие методы для обновления, удаления, поиска товаров и т.д.
        }
    }
