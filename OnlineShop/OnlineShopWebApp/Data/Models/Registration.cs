using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public class Registration
    {
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email", Prompt = "example@email.com")]
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Пароль должен содержать от 8 до 16 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Пароль должен содержать цифры, заглавные и строчные буквы")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля", Prompt = "Повторите пароль")]
        public string ConfirmPassword { get; set; }
    }
}