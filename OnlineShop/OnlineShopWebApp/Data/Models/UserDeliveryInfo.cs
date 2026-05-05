using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public class UserDeliveryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}