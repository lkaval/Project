using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShopWebApp.Data.Models
{
    public class UserDeliveryInfo
    {
        [Required(ErrorMessage = "Поле ФИО обязательно для заполнения")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ФИО должно содержать от 3 до 100 символов")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z\s\-]+$", ErrorMessage = "ФИО может содержать только буквы, пробелы и дефисы")]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Телефон обязательно для заполнения")]
        [RegularExpression(@"^(\+7|8)[0-9]{10}$", ErrorMessage = "Номер телефона должен начинаться с +7 или 8 и содержать 11 цифр")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле Адрес обязательно для заполнения")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Адрес должен содержать от 10 до 200 символов")]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z0-9\s\-\,\.\/]+$", ErrorMessage = "Адрес содержит недопустимые символы")]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }

    }
}