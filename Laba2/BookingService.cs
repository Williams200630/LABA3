public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IPricingService _pricingService;

    public BookingService(IBookingRepository bookingRepository,
                         IRoomRepository roomRepository,
                         IClientRepository clientRepository,
                         IPricingService pricingService)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _clientRepository = clientRepository;
        _pricingService = pricingService;
    }

    // Пример метода создания бронирования
    public async Task<BookingResult> CreateBookingAsync(BookingRequestDto bookingRequest)
    {
        if (!ValidationService.ValidateBookingRequest(bookingRequest))
            return BookingResult.Failure("Некорректные данные запроса.");

        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(
            bookingRequest.CheckInDate, bookingRequest.CheckOutDate);

        var selectedRoom = availableRooms
            .FirstOrDefault(r => r.Category == bookingRequest.RoomCategory);

        if (selectedRoom == null)
            return BookingResult.Failure("Нет свободных номеров выбранной категории.");

        decimal totalCost = await _pricingService.CalculateBookingCostAsync(new Booking
        {
            Room = selectedRoom,
            CheckInDate = bookingRequest.CheckInDate,
            CheckOutDate = bookingRequest.CheckOutDate,
            Services = bookingRequest.Services,
            ClientId = bookingRequest.ClientId
        });

        var booking = new Booking
        {
            RoomId = selectedRoom.RoomId,
            Status = BookingStatus.Pending,
            CheckInDate = bookingRequest.CheckInDate,
            CheckOutDate = bookingRequest.CheckOutDate,
            ClientId = bookingRequest.ClientId,
            TotalCost = totalCost
        };

        await _bookingRepository.AddAsync(booking);
        Logger.LogInfo($"Создано новое бронирование: КлиентId={booking.ClientId}, Номер={selectedRoom.RoomId}");
        await NotificationService.SendBookingConfirmationAsync(bookingRequest.ClientEmail, booking.BookingId);

        return BookingResult.Success(booking.BookingId, totalCost);
    }
}
