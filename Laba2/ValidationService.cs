using System;
using System.Text.RegularExpressions;

namespace HotelManagementSystem.Services
{
    public class ValidationService
    {
        // Email-валидация, стандарт RFC
        public bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        // Проверка дат проживания: дата заезда/выезда и будущие даты
        public bool ValidateDates(DateTime checkIn, DateTime checkOut)
        {
            return checkIn >= DateTime.Today && checkOut > checkIn;
        }

        // Проверка паспорта: длина = 10, все цифры
        public bool ValidatePassport(string passportNumber)
        {
            return !string.IsNullOrWhiteSpace(passportNumber)
                   && passportNumber.Length == 10
                   && passportNumber.All(char.IsDigit);
        }

        // Проверка статуса клиента (например, заблокирован)
        public bool ValidateClient(Client client)
        {
            return client != null && client.IsActive && !client.IsBlacklisted;
        }

        // Общая проверка для запроса на бронирование
        public bool ValidateBookingRequest(BookingRequestDto dto)
        {
            return ValidateEmail(dto.ClientEmail)
                && ValidateDates(dto.CheckInDate, dto.CheckOutDate)
                && dto.RoomCategory > 0;
        }
    }
}
