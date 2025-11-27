// Booking.cs - Hotel Booking System
// Version 1.0
// Author: Bandurko
// Date: 27.11.2025

using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    // Возможные статусы бронирования
    public enum BookingStatus
    {
        Pending,     // Ожидание
        Confirmed,   // Подтверждено
        CheckedIn,   // Заселен
        CheckedOut,  // Выселен
        Cancelled    // Отменен
    }

    // Класс для хранения всей информации о бронировании
    public class Booking
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }                     // Ссылка на клиента
        public int RoomId { get; set; }
        public Room Room { get; set; }                         // Ссылка на номер
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalCost { get; set; }
        public decimal PaidAmount { get; set; }
        public List<AdditionalService> Services { get; set; }

        // Количество ночей
        public int Nights => (CheckOutDate - CheckInDate).Days;

        // Остаток к оплате
        public decimal Balance => TotalCost - PaidAmount;

        public Booking()
        {
            Services = new List<AdditionalService>();
            BookingDate = DateTime.Now;
        }
    }
}
