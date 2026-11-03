
using Microsoft.AspNetCore.Mvc.RazorPages;
using PMS_Lab_8.Data;
using PMS_Lab_8.Model;

namespace PMS_Lab_8.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductRepository _repository;

        public IndexModel(ProductRepository repository)
        {
            _repository = repository;
        }

        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _repository.GetAllAsync();
        }
    }
}
