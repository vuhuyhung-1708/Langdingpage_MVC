namespace website_test.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using website_test.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<website_test.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(website_test.Models.ApplicationDbContext context)
        {
            // ✅ Thêm dữ liệu vào bảng UserMessages
            context.UserMessages.AddOrUpdate(
                m => m.Email,  // Kiểm tra trùng email để không bị thêm lặp
                new UserMessage { Name = "Nguyen Van A", Email = "nguyenvana@example.com", Message = "Hello from ASP.NET MVC!" },
                new UserMessage { Name = "Tran Thi B", Email = "tranthib@example.com", Message = "This is another test message." },
                new UserMessage { Name = "Le Van C", Email = "levanc@example.com", Message = "New user added via Seed()" },
                new UserMessage { Name = "Pham Thi D", Email = "phamthid@example.com", Message = "Testing seeding more data" }
            );

            context.SaveChanges();  // ✅ Lưu vào database

            // ✅ In dữ liệu ra Console để kiểm tra
            var messages = context.UserMessages.ToList();
            Console.WriteLine("🔹 Danh sách tin nhắn:");
            foreach (var msg in messages)
            {
                Console.WriteLine($"ID: {msg.Id}, Name: {msg.Name}, Email: {msg.Email}, Message: {msg.Message}");
            }
        }
    }
}
