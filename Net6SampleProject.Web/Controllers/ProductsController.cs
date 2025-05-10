using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Net6SampleProject.MVC.Models.Configurations;
using Net6SampleProject.MVC.Models.Responses;
using Net6SampleProject.MVC.Models.ViewModels;

namespace Net6SampleProject.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _productUrl;

        public ProductsController(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _productUrl = $"{options.Value.BaseUrl}products";
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<IEnumerable<ProductViewModel>>>($"{_productUrl}");
            if (response == null || response.Data == null)
                return View(Enumerable.Empty<ProductViewModel>());

            return View(response.Data);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<ProductViewModel>>($"{_productUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _httpClient.PostAsJsonAsync(_productUrl, model);
            if (!result.IsSuccessStatusCode)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<ProductViewModel>>($"{_productUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // POST: /Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _httpClient.PutAsJsonAsync(_productUrl, model);
            if (!result.IsSuccessStatusCode)
                return View(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<ProductViewModel>>($"{_productUrl}/{id}");
            if (response == null || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        // POST: /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _httpClient.DeleteAsync($"{_productUrl}/{id}");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/WithCategory
        public async Task<IActionResult> WithCategory()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponse<IEnumerable<ProductsWithCategoryViewModel>>>($"{_productUrl}/getproductswithcategory");
            if (response == null || response.Data == null)
                return View(Enumerable.Empty<ProductsWithCategoryViewModel>());

            return View(response.Data);
        }
    }
}
