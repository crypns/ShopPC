using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopPC
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Конструктор по умолчанию
        public Category()
        {
        }

        // Конструктор с параметрами
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        // Метод для добавления категории в базу данных
        public void AddToDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Categories WHERE Name = @Name";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", Name);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // Категория с таким именем уже существует, выбрасываем исключение
                        throw new InvalidOperationException("Категория с таким именем уже существует.");
                    }
                    else
                    {
                        // Категории с таким именем нет, добавляем новую
                        string insertQuery = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)";
                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", Name);
                            command.Parameters.AddWithValue("@Description", Description);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }




        // Метод для обновления информации о категории в базе данных
        public void UpdateInDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.ExecuteNonQuery();
                }
            }
        }


        // Метод для удаления категории из базы данных
        public void DeleteFromDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Categories WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Category> SearchCategories(string searchQuery, string connectionString)
        {
            List<Category> searchResults = new List<Category>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Description FROM Categories WHERE Name LIKE @SearchQuery OR Description LIKE @SearchQuery";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = reader["Name"].ToString();
                            string description = reader["Description"].ToString();

                            searchResults.Add(new Category { Id = id, Name = name, Description = description });
                        }
                    }
                }
            }

            return searchResults;
        }


        // Метод для получения списка всех категорий из базы данных
        public static List<Category> GetAllCategories()
        {
            // Реализация запроса всех категорий из базы данных
            List<Category> categories = new List<Category>();
            // Здесь нужно написать код для запроса категорий
            return categories;
        }
    }

}
