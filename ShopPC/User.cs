using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPC
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public User()
        {
        }

        public User(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public static class UserRole
        {
            public const string Administrator = "Администратор";
            public const string Seller = "Продавец";
        }

        public void AddToDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Users (Name, Role) VALUES (@Name, @Role)";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateInDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Users SET Name = @Name, Role = @Role WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFromDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Users WHERE Id = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<User> SearchUsers(string searchQuery, string connectionString)
        {
            List<User> searchResults = new List<User>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Role FROM Users WHERE Name LIKE @SearchQuery OR Role LIKE @SearchQuery";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string name = reader["Name"].ToString();
                            string role = reader["Role"].ToString();

                            searchResults.Add(new User { Id = id, Name = name, Role = role });
                        }
                    }
                }
            }

            return searchResults;
        }

        public static List<User> GetAllUsers(string connectionString)
        {
            List<User> users = new List<User>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Role FROM Users";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Id"]);
                        string name = reader["Name"].ToString();
                        string role = reader["Role"].ToString();

                        users.Add(new User { Id = id, Name = name, Role = role });
                    }
                }
            }

            return users;
        }
    }
}
