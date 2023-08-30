using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ShopPC
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Конструктор по умолчанию
        public Supplier()
        {
        }

        // Конструктор с параметрами
        public Supplier(string title, string name, string phone, string address)
        {
            Title = title;
            Name = name;
            Phone = phone;
            Address = address;
        }

        // Метод для добавления поставщика в базу данных
        public void AddToDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Suppliers (Title, Name, Phone, Address) VALUES (@Title, @Name, @Phone, @Address)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Метод для обновления информации о поставщике в базе данных
        public void UpdateInDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Suppliers SET Title = @Title, Name = @Name, Phone = @Phone, Address = @Address WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Метод для удаления поставщика из базы данных
        public void DeleteFromDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Suppliers WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Метод для поиска поставщиков по названию или имени
        public static List<Supplier> SearchSuppliers(string searchQuery, string connectionString)
        {
            List<Supplier> searchResults = new List<Supplier>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Title, Name, Phone, Address FROM Suppliers WHERE Title LIKE @SearchQuery OR Name LIKE @SearchQuery OR Phone LIKE @SearchQuery OR Address LIKE @SearchQuery";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string title = reader["Title"].ToString();
                            string name = reader["Name"].ToString();
                            string phone = reader["Phone"].ToString();
                            string address = reader["Address"].ToString();

                            searchResults.Add(new Supplier { Id = id, Title = title, Name = name, Phone = phone, Address = address });
                        }
                    }
                }
            }

            return searchResults;
        }

        // Метод для получения списка всех поставщиков из базы данных
        public static List<Supplier> GetAllSuppliers(string connectionString)
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Title, Name, Phone, Address FROM Suppliers";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string title = reader["Title"].ToString();
                            string name = reader["Name"].ToString();
                            string phone = reader["Phone"].ToString();
                            string address = reader["Address"].ToString();

                            suppliers.Add(new Supplier { Id = id, Title = title, Name = name, Phone = phone, Address = address });
                        }
                    }
                }
            }

            return suppliers;
        }
    }
}
