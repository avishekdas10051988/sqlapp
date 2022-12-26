using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sqlapp.Models;
using sqlapp.Services;

namespace sqlapp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Product> products = new List<Product>();
        public bool isBeta = false;

        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public void OnGet()
        {
            isBeta = _productService.IsBeta().Result;
            products = _productService.GetProducts().GetAwaiter().GetResult();
        }
    }
}