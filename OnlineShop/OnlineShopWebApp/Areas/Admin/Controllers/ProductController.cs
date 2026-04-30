using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Products;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductsRepository productsRepository, IWebHostEnvironment env)
        {
            _productsRepository = productsRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products);
        }

        public IActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50_000_000)] // 50 MB limit
        public async Task<IActionResult> Add(Product product, IFormFile? imageFile)
        {
            // 🔹 Логирование для отладки
            System.Diagnostics.Debug.WriteLine($"[DEBUG] === START Add === File: {imageFile?.FileName ?? "NULL"}");

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] ModelState invalid");
                return View(product);
            }

            try
            {
                // 🔹 1. Проверяем, что WebRootPath не null
                if (string.IsNullOrEmpty(_env.WebRootPath))
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] WebRootPath is NULL! Using fallback path.");
                    // Фоллбэк: используем папку в корне проекта, если wwwroot не найден
                    _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                // 🔹 2. Обрабатываем файл, если он есть
                if (imageFile != null && imageFile.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Processing file: {imageFile.FileName}, Size: {imageFile.Length}");

                    // Валидация расширения
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Недопустимый формат. Используйте JPG, PNG или WEBP.");
                        return View(product);
                    }

                    // Генерация безопасного имени файла
                    var uniqueFileName = Guid.NewGuid().ToString() + extension;

                    // 🔹 3. Формируем путь и создаем папку
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Target folder: {uploadsFolder}");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Creating directory: {uploadsFolder}");
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Saving to: {filePath}");

                    // 🔹 4. Сохраняем файл (через CopyToAsync)
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    System.Diagnostics.Debug.WriteLine("[DEBUG] File saved successfully!");
                    product.ImagePath = $"/images/{uniqueFileName}";
                }
                else
                {
                    product.ImagePath = "/images/default_product.jpg";
                }

                // 🔹 5. Сохраняем товар в БД
                System.Diagnostics.Debug.WriteLine("[DEBUG] Saving product to DB...");
                _productsRepository.Add(product);

                System.Diagnostics.Debug.WriteLine("[DEBUG] === SUCCESS ===");
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Access Denied: {ex.Message}");
                ModelState.AddModelError("", $"Ошибка доступа к файлам: {ex.Message}. Проверьте права на папку wwwroot/images");
                return View(product);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] IO Error: {ex.Message}");
                ModelState.AddModelError("", $"Ошибка записи файла: {ex.Message}");
                return View(product);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CRITICAL ERROR] {ex.GetType().Name}: {ex.Message}\nStack: {ex.StackTrace}");
                ModelState.AddModelError("", $"Неизвестная ошибка: {ex.Message}");
                return View(product);
            }
        }

        public IActionResult Edit(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(product);

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (new[] { ".jpg", ".jpeg", ".png", ".webp" }.Contains(extension))
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + extension;
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await imageFile.CopyToAsync(stream);

                        product.ImagePath = $"/images/{uniqueFileName}";
                    }
                }
                _productsRepository.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR Edit] {ex.Message}");
                ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                return View(product);
            }
        }
    }
}