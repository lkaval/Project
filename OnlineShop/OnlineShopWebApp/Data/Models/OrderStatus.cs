using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Data.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Создан")]
        Created,

        [Display(Name = "Обработан")]
        Processed,

        [Display(Name = "В пути")]
        Deliviring,

        [Display(Name = "Доставлен")]
        Delivered,

        [Display(Name = "Отменен")]
        Canceled
    }
}