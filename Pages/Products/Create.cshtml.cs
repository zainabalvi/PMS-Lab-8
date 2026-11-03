
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PMS_Lab_8.Data;
using PMS_Lab_8.Model;

namespace PMS_Lab_8.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductRepository _repository;
        private readonly ProductImageService _images;

        public CreateModel(
            ProductRepository repository,
            ProductImageService images)
        {
            _repository = repository;
            _images = images;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public void OnGet()
        {
            Product.status = "Available";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string? error = _images.Validate(ImageFile);

                if (error != null)
                {
                    ModelState.AddModelError(nameof(ImageFile), error);
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string savedImage = string.Empty;

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    savedImage = await _images.SaveAsync(ImageFile);
                    Product.image = savedImage;
                }

                await _repository.CreateAsync(Product);

                TempData["Success"] = "Product added successfully.";
                return RedirectToPage("Index");
            }
            catch
            {
                if (!string.IsNullOrEmpty(savedImage))
                {
                    _images.Delete(savedImage);
                }

                throw;
            }
        }
    }
}
