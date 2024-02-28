using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepo;
        private readonly IRepository<ProductEntity> _productRepo;

        public CategoryManager(IRepository<CategoryEntity> categoryRepo, IRepository<ProductEntity> productRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;

        }
        public bool AddCategory(CategoryAddDto categoryAddDto)
        {
            var hasCategory = _categoryRepo.GetAll(x => x.Name.ToLower() == categoryAddDto.Name.ToLower()).ToList();

            if(hasCategory.Any()) 
            {
                return false;
            }

            var entity = new CategoryEntity()
            {
                Name = categoryAddDto.Name,
                Description = categoryAddDto.Description,

            };

            _categoryRepo.Add(entity);
            return true;

        }

        public bool DeleteCategory(int id)
        {

            var hasProduct = _productRepo.GetAll(x => x.Id == id).ToList(); 

            if(hasProduct.Any())
            {
                return false;
            }

            _categoryRepo.Delete(id);
            return true;

        }

        public List<CategoryListDto> GetAllCategories()
        {
            var categoryEntities = _categoryRepo.GetAll().OrderBy(x => x.Name);

            var categoryListDto = categoryEntities.Select(x => new CategoryListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,

            }).ToList();

            return categoryListDto;

        }

        public CategoryUpdateDto GetCategory(int id)
        {
            var entity = _categoryRepo.GetById(id);

            var categoryUpdateDto = new CategoryUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
            };

            

            return categoryUpdateDto;
        }

        public bool UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var hasCategory = _categoryRepo.GetAll(x => x.Name.ToLower() == categoryUpdateDto.Name.ToLower() && x.Id != categoryUpdateDto.Id).ToList(); ;

            if (hasCategory.Any())
            {
                return false;
            }

            var entity = _categoryRepo.GetById(categoryUpdateDto.Id);

            entity.Name = categoryUpdateDto.Name;
            entity.Description = categoryUpdateDto.Description;

            _categoryRepo.Update(entity);

            return true;

        }
    }
}
