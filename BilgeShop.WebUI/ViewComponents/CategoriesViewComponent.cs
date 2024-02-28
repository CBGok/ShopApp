using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService; 
        }

        public IViewComponentResult Invoke()
        {
            var categoryDtos = _categoryService.GetAllCategories();

            var viewModel = categoryDtos.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();

            return View(viewModel);
        }

    }
}
