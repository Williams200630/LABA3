using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Services
{
    public interface IPricingService
    {
        Task<decimal> CalculateBookingCostAsync(Booking booking);
        decimal CalculateServicesCost(Booking booking);
        decimal ApplyLoyaltyDiscount(decimal total, decimal discount);
        decimal ApplyPromotion(decimal total, Promotion promotion);
    }

    public class PricingService : IPricingService
    {
        private readonly IClientRepository _clientRepository;

        public PricingService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Основной расчет общей стоимости
        public async Task<decimal> CalculateBookingCostAsync(Booking booking)
        {
            int nights = (booking.CheckOutDate - booking.CheckInDate).Days;
            decimal roomCost = booking.Room.BasePrice * nights;
            decimal servicesCost = CalculateServicesCost(booking);
            decimal totalCost = roomCost + servicesCost;

            var client = await _clientRepository.GetByIdAsync(booking.ClientId);
            if (client != null && client.LoyaltyDiscount > 0)
                totalCost = ApplyLoyaltyDiscount(totalCost, client.LoyaltyDiscount);

            if (booking.Promotion != null)
                totalCost = ApplyPromotion(totalCost, booking.Promotion);

            return totalCost;
        }

        // Подсчёт стоимости всех доп. услуг
        public decimal CalculateServicesCost(Booking booking)
        {
            return booking.Services?.Sum(s => s.Price) ?? 0;
        }

        // Применение скидки программы лояльности
        public decimal ApplyLoyaltyDiscount(decimal total, decimal discount)
        {
            return total - total * discount / 100;
        }

        // Применение промоакций, если есть
        public decimal ApplyPromotion(decimal total, Promotion promotion)
        {
            return Math.Max(0, total - promotion.DiscountAmount);
        }
    }
}
