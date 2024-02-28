using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public ProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(int? categoryId = null)
        {
            var productDtos = _productService.GetAllProductsByCategoryId(categoryId);

            var viewModel = productDtos.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                ImagePath = x.ImagePath,
                CategoryName = x.CategoryName,
            }).ToList();


            return View(viewModel);
        }

    }
}
