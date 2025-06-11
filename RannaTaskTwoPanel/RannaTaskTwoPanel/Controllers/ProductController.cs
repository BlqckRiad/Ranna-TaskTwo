using Microsoft.AspNetCore.Mvc;
using RannaTaskTwoPanel.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RannaTaskTwoPanel.Controllers
{
    public class ProductController : Controller
    {
        private readonly string apiBase = "https://localhost:7014/api/";

        private void SetToken(HttpClient client)
        {
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IActionResult> Index()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);
            SetToken(client);

            var products = await client.GetFromJsonAsync<List<Product>>("products");
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);
            SetToken(client);

            var dto = new ProductCreateDto
            {
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                ImagePath = product.ImagePath
            };

            var response = await client.PostAsJsonAsync("products", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Create failed");
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);
            SetToken(client);

            var product = await client.GetFromJsonAsync<Product>($"products/{id}");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);
            SetToken(client);

            var response = await client.PutAsJsonAsync($"products/{product.Id}", product);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Update failed");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);
            SetToken(client);

            var response = await client.DeleteAsync($"products/{id}");
            return RedirectToAction("Index");
        }
    }
} 