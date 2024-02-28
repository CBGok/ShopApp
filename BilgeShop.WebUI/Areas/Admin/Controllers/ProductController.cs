using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;
        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;

        }


        public IActionResult List()
        {
            var productDtoList = _productService.GetAllProducts();

            var viewModel = productDtoList.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryName = x.CategoryName,
                ImagePath = x.ImagePath,
                UnitsInStock = x.UnitsInStock,
                UnitPrice = x.UnitPrice,

            }).ToList();

            return View(viewModel);
        }

        public IActionResult New()
        {

            ViewBag.Categories = _categoryService.GetAllCategories();
            return View("Form",new ProductFormNewModel());
        }

        public IActionResult Update(int id)
        {
            var dto = _productService.GetProductById(id);

            var viewModel = new ProductFormNewModel()
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                UnitPrice = dto.UnitPrice,
                UnitsInStock = dto.UnitsInStock,
                CategoryId = dto.CategoryId,
            };

            ViewBag.ImagePath = dto.ImagePath;
            ViewBag.Categories = _categoryService.GetAllCategories();
            return View("Form", viewModel);
           
        }

        public IActionResult Save(ProductFormNewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAllCategories();
                return View("Form", formData);
            }

            var newFileName = "";

            if(formData.File is not null)
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/jfif", "image/avif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif", ".avif" };

                var fileContentType = formData.File.ContentType;

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);

                var fileExtension = Path.GetExtension(formData.File.FileName);

                if (!allowedFileTypes.Contains(fileContentType) ||
                    !allowedFileExtensions.Contains(fileExtension))
                {

                    ViewBag.ImageErrorMessage = "Yüklediğiniz dosya" + fileExtension + " uzantısında. Sisteme yalnızca .jpg .pjeg .png .jfif .avif formatında dosyalar yüklenebilir.";
                    ViewBag.Categories = _categoryService.GetAllCategories();
                    return View("Form", formData);
                }

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;

                var folderPath = Path.Combine("images", "products");

                var wwwrootFolderPath = Path.Combine(_environment.WebRootPath, folderPath); // tak tak

                var filePath = Path.Combine(wwwrootFolderPath, newFileName); // tak tak tak

                Directory.CreateDirectory(wwwrootFolderPath);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formData.File.CopyTo(fileStream);
                }
            }

            if(formData.Id == 0)
            {
                //ekleme
                var productAddDto = new ProductAddDto()
                {
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitsInStock = formData.UnitsInStock,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName,
                };

                _productService.AddProduct(productAddDto);
                return RedirectToAction("List");
            }
            else
            {
                var dto = new ProductUpdateDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitsInStock = formData.UnitsInStock,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName
                };

                _productService.UpdateProduct(dto);
                return RedirectToAction("List");
            }

            
        }


        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("List");
        }
    }
}
