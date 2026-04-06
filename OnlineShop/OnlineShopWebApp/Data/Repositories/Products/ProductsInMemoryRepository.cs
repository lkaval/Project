using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Products
{
    public class ProductsInMemoryRepository : IProductsRepository
    {
        private List<Product> products = new List<Product>()
        {
            new Product("Elden Ring", 1999,
                "Elden Ring — это захватывающая action-RPG, разработанная компанией FromSoftware в сотрудничестве с Джорджем Р. Р. Мартином.\n\n" +
                "Игра сочетает в себе сложный геймплей в стиле Souls с открытым миром, полным тайн, опасностей и эпических битв.\n\n" +
                "Исследуйте огромные земли, сражайтесь с могущественными врагами и раскрывайте секреты этого загадочного мира.",
                "/images/Elden_Ring_Logo.jpg"),

            new Product("Cyberpunk 2077", 1299,
                "Cyberpunk 2077 — это ролевая игра в открытом мире, действие которой происходит в городе Найт-Сити, где царит хаос, технологии и модификации тела.\n\n" +
                "Играйте за V, наемника, который ищет уникальный имплант, способный даровать бессмертие.\n\n" +
                "Вас ждут сложные решения, динамичные боевые системы и глубокий сюжет, который меняется в зависимости от ваших действий.",
                "/images/Cyberpunk_2077_Logo.jpg"),

            new Product("Baldur's Gate 3", 1999,
                "Baldur's Gate 3 — это эпическая ролевая игра, разработанная Larian Studios, основанная на правилах Dungeons & Dragons 5-й редакции.\n\n" +
                "Погрузитесь в мир, полный магии, интриг и опасностей, где каждое ваше решение влияет на развитие сюжета.\n\n" +
                "Создайте своего уникального героя, соберите отряд и отправляйтесь в захватывающее приключение, где вас ждут битвы, загадки и моральные дилеммы.",
                "/images/Baldurs_Gate_3_Logo.jpg"),

            new Product("Path of Exile 2", 1499,
                "Path of Exile 2 — это продолжение культовой action-RPG, которая предлагает игрокам мрачный и сложный мир, наполненный монстрами, сокровищами и бесконечными возможностями для кастомизации персонажа.\n\n" +
                "Новые классы, улучшенная графика и глубокая система прокачки делают эту игру идеальным выбором для любителей жанра.\n\n" +
                "Сражайтесь с ордами врагов, исследуйте опасные подземелья и создайте своего уникального героя.",
                "/images/Path_Of_Exile_2_Logo.jpg"),
        };
        public void Add(Product product)
        {

            product.ImagePath = "/images/Cyberpunk_2077_Logo.jpg";
            products.Add(product);

        }

        public List<Product> GetAll()
        {
            return products;
        }

        public Product TryGetById(int id)
        {
            return products.FirstOrDefault(product => product.Id == id);
        }

        public void Update(Product product)
        {
            var existingProduct = products.FirstOrDefault(x => x.Id == product.Id);
            if (existingProduct == null)
            {
                return;

            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Cost = product.Cost;
        }
    }
}