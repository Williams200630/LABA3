using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface INotificationService
    {
        Task SendBookingConfirmationAsync(string clientEmail, int bookingId);
        Task SendCheckInNotificationAsync(string clientEmail, int bookingId);
        Task SendCheckOutNotificationAsync(string clientEmail, int bookingId);
        Task SendDebtReminderAsync(string clientEmail, decimal sum);
        Task SendPromotionAsync(string clientEmail, string text);
    }

    public class NotificationService : INotificationService
    {
        // Реализация через SMTP, SMS или сторонний API

        public async Task SendBookingConfirmationAsync(string clientEmail, int bookingId)
        {
            string subject = $"Подтверждение бронирования №{bookingId}";
            string body = $"Бронирование успешно подтверждено!";
            await SendEmail(clientEmail, subject, body);
        }

        public async Task SendCheckInNotificationAsync(string clientEmail, int bookingId)
        {
            await SendEmail(clientEmail, $"Заселение №{bookingId}", $"Вы успешно заселены!");
        }

        public async Task SendCheckOutNotificationAsync(string clientEmail, int bookingId)
        {
            await SendEmail(clientEmail, $"Выселение №{bookingId}", $"Спасибо за посещение! Ожидаем снова.");
        }

        public async Task SendDebtReminderAsync(string clientEmail, decimal sum)
        {
            await SendEmail(clientEmail, $"Напоминание о долге", $"Пожалуйста, оплатите задолженность {sum:C}");
        }

        public async Task SendPromotionAsync(string clientEmail, string text)
        {
            await SendEmail(clientEmail, $"Специальное предложение", text);
        }

        private async Task SendEmail(string to, string subject, string body)
        {
            // Здесь код отправки email (SMTP, Mailgun, SendGrid и др.)
            // await smtpClient.SendMailAsync(message);
        }
    }
}
