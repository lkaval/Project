using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public class Product
    {
        private static int instanceCounter = 0;
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите название товара")]
        [StringLength(100, ErrorMessage = "Название товара не может превышать 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите цену товара")]
        [Range(0, 1000000,
            ErrorMessage = "Цена должна быть не менее 0 ₽ и не более 1 000 000 ₽")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Добавьте описание товара")]
        [StringLength(1000,
            ErrorMessage = "Описание слишком длинное (максимум 1000 символов)")]
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public Product()
        {
            Id = instanceCounter;
            instanceCounter ++;
        }

        public Product(string name, decimal cost, string description, string imagePath) : this()
        {
            Name = name;
            Cost = cost;
            Description = description;
            ImagePath = imagePath;
        }

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}";
        }
    }
}