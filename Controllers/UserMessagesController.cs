using System;
using System.Linq;
using System.Web.Mvc;
using website_test.Models;

namespace website_test.Controllers
{
    public class UserMessagesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext(); 

        // Hiển thị danh sách tin nhắn
        public ActionResult Index()
        {
            var messages = _context.UserMessages.ToList();
            return View(messages);
        }

        [HttpPost]
        public ActionResult Submit(UserMessage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserMessages.Add(model);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Gửi thành công!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Gửi thất bại! Lỗi: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
            }

            return RedirectToAction("Index", "Home");
        }

        // Hàm xử lý xóa dữ liệu
        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            try
            {
                var message = _context.UserMessages.Find(id);
                if (message != null)
                {
                    _context.UserMessages.Remove(message);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Xóa thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tin nhắn!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Xóa thất bại! Lỗi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // Gửi email cảm ơn
        [HttpPost]
        public ActionResult SendReplyEmail(int id)
        {
            Debug.WriteLine("🟢 Đã gọi đến SendReplyEmail với ID: " + id);

            var message = _context.UserMessages.Find(id);
            if (message == null)
            {
                Debug.WriteLine("🔴 Không tìm thấy tin nhắn!");
                TempData["ErrorMessage"] = "Không tìm thấy tin nhắn!";
                return RedirectToAction("Index");
            }

            try
            {
                string subject = "Cảm ơn bạn đã liên hệ!";
                string body = $"Xin chào {message.Name},\n\n" +
                              "Cảm ơn bạn đã quan tâm đến dịch vụ của chúng tôi!\n\n" +
                              "Trân trọng,\nDịch vụ Hỗ trợ";

                bool emailSent = SendEmail(message.Email, subject, body);

                if (emailSent)
                {
                    Debug.WriteLine("✅ Email đã gửi thành công!");
                    TempData["SuccessMessage"] = "Email cảm ơn đã được gửi thành công!";
                }
                else
                {
                    Debug.WriteLine("❌ Không thể gửi email!");
                    TempData["ErrorMessage"] = "Không thể gửi email!";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("❌ Lỗi khi gửi email: " + ex.Message);
                TempData["ErrorMessage"] = "Lỗi khi gửi email: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        private bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromEmail = "your-email@gmail.com";
                string password = "your-app-password";

                var fromAddress = new MailAddress(fromEmail, "Dịch vụ Hỗ trợ");
                var toAddress = new MailAddress(toEmail);

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromEmail, password);

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    })
                    {
                        smtp.Send(message);
                    }
                }
                Debug.WriteLine("✅ Email đã gửi thành công!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("❌ Lỗi khi gửi email: " + ex.Message);
                return false;
            }
      
        }



}
}

