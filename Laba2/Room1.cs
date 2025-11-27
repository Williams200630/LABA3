namespace HotelManagementSystem.Models
{
    public enum RoomStatus
    {
        Available,      // Свободен
        Occupied,       // Занят
        Reserved,       // Забронирован
        Maintenance     // На обслуживании
    }

    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int Category { get; set; }
        public RoomStatus Status { get; set; }
        public decimal BasePrice { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
    }
}
