using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Net6SampleProject.MVC.Models.Configurations;
using Net6SampleProject.MVC.Models.Responses;
using Net6SampleProject.MVC.Models.ViewModels;

namespace Net6SampleProject.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _categoryUrl;

        public CategoriesController(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _categoryUrl = $"{options.Value.BaseUrl}categories";
        }

        // GET: /Categories
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<IEnumerable<CategoryViewModel>>>($"{_categoryUrl}");
            if (response == null || response.Data == null)
                return View(Enumerable.Empty<CategoryViewModel>());

            return View(response.Data);
        }

        // GET: /Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<CategoryViewModel>>($"{_categoryUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // GET: /Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _httpClient.PostAsJsonAsync(_categoryUrl, model);
            if (!result.IsSuccessStatusCode)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<CategoryViewModel>>($"{_categoryUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // POST: /Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _httpClient.PutAsJsonAsync(_categoryUrl, model);
            if (!result.IsSuccessStatusCode)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<CategoryViewModel>>($"{_categoryUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // POST: /Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _httpClient.DeleteAsync($"{_categoryUrl}/{id}");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Categories/WithProducts/5
        public async Task<IActionResult> WithProducts(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<CategoryWithProductsViewModel>>($"{_categoryUrl}/GetCategoryWithProducts/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }
    }
}
