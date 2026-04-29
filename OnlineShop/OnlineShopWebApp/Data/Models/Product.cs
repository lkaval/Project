using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public class Product
    {
        public int Id { get; set; } // ← EF Core сам заполнит через ValueGeneratedOnAdd

        [Required(ErrorMessage = "Пожалуйста, укажите название товара")]
        [StringLength(100, ErrorMessage = "Название товара не может превышать 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите цену товара")]
        [Range(0, 1000000, ErrorMessage = "Цена должна быть не менее 0 ₽ и не более 1 000 000 ₽")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Добавьте описание товара")]
        [StringLength(1000, ErrorMessage = "Описание слишком длинное (максимум 1000 символов)")]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        // Пустой конструктор для EF Core
        public Product() { }

        // Конструктор для создания новых товаров (без задания Id!)
        public Product(string name, decimal cost, string description, string imagePath)
        {
            Name = name;
            Cost = cost;
            Description = description;
            ImagePath = imagePath;
            // Id не задаём — его присвоит БД
        }

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}";
        }
    }
}