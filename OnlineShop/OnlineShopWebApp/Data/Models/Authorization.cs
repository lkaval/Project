using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public class Authorization
    {
        [Required(ErrorMessage = "Пожалуйста, введите ваш email")]
        [EmailAddress(ErrorMessage = "Формат email недействителен")]
        [Display(Name = "Email", Prompt = "example@email.com")]
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш пароль")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Пароль должен содержать от 8 до 16 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите ваш пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}