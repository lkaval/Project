using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
    public class Role
    {
        [Required(ErrorMessage = "Название роли обязательно для заполнения")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название роли должно содержать от 3 до 50 символов")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ0-9_\s-]+$",
            ErrorMessage = "Название роли может содержать только буквы, цифры, пробелы, дефисы и подчеркивания")]
        [Display(Name = "Название роли")]
        public string Name { get; set; }


    }
}