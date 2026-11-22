using Talabat.APIs.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class CustmorBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public List<BasketItemDto> Items { get; set; } = null!;
        public int? DeliveryMethodsId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
