using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace website_test.Models
{
    [Table("user_messages")]
    public class UserMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        private static string dbPath = HttpContext.Current.Server.MapPath("~/App_Data/Database.sqlite");
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        // Hàm tạo database và bảng user_messages
        public static void InitializeDatabase()
        {
            try
            {
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        CREATE TABLE IF NOT EXISTS user_messages (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL,
                            email TEXT NOT NULL,
                            message TEXT NOT NULL
                        );";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi khởi tạo database: {ex.Message}");
            }
        }

        // Hàm thêm dữ liệu vào bảng user_messages
        public static void InsertMessage(string name, string email, string message)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO user_messages (name, email, message) VALUES (@name, @email, @message)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@message", message);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        //// Hàm cập nhật dữ liệu trong bảng user_messages
        //public static void UpdateMessage(int id, string name, string email, string message)
        //{
        //    using (var connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "UPDATE user_messages SET name = @name, email = @email, message = @message WHERE id = @id";
        //        using (var command = new SQLiteCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@id", id);
        //            command.Parameters.AddWithValue("@name", name);
        //            command.Parameters.AddWithValue("@email", email);
        //            command.Parameters.AddWithValue("@message", message);
        //            command.ExecuteNonQuery();
        //        }

        //        connection.Close();
        //    }
        //}

        // Hàm xóa dữ liệu trong bảng user_messages
        public static void DeleteMessage(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM user_messages WHERE id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
