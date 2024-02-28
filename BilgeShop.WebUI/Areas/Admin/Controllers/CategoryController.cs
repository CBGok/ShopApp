using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;    
        }


        public IActionResult List()
        {
            var categoryListDto = _categoryService.GetAllCategories();

            var viewModel = categoryListDto.Select(x => new CategoryListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description?.Length > 20 ? x.Description?.Substring(0, 20) + "..." : x.Description
            }).ToList();


            return View(viewModel); // foreachle dönemeyiz içine yazmazsak
        }

        public IActionResult New()
        {
            return View("Form",new CategoryFormViewModel());
        }

        public IActionResult Update(int id)
        {
            var categoryDto = _categoryService.GetCategory(id);

            var viewModel = new CategoryFormViewModel()
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };


            return View("Form", viewModel);
        }

        [HttpPost]
        public IActionResult Save(CategoryFormViewModel formData) 
        {
            if (!ModelState.IsValid)
            {
                return View("Form", formData);
            }

            if(formData.Id == 0) // yeni ekleme işlemi
            {
                var categoryAddDto = new CategoryAddDto()
                {
                    Name = formData.Name,
                    Description = formData.Description?.Trim()
                };

                var result = _categoryService.AddCategory(categoryAddDto);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut.";
                    return View("Form", formData);
                }

            }
            else  // güncelleme işlemi
            {
                var categoryUpdateDto = new CategoryUpdateDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description
                };

                var result = _categoryService.UpdateCategory(categoryUpdateDto);

                if (!result)
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut olduğundan, güncelleme yapamazsınız.";
                    return View("Form", formData);
                }

                return RedirectToAction("List");

            }



        }

        public IActionResult Delete(int id)
        {
            
           var result = _categoryService.DeleteCategory(id);

            if (!result)
            {
                TempData["Message"] = "Kategoride ürün var";
                return View("List");
            }
            else
            {
                return RedirectToAction("List");
            }

           
        }
    }
}
