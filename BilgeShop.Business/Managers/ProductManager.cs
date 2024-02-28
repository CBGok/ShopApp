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
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepo;

        public ProductManager(IRepository<ProductEntity> productRepo)
        {
           _productRepo = productRepo;
        }

        public void AddProduct(ProductAddDto productAddDto)
        {
            var entity = new ProductEntity()
            {
                Name = productAddDto.Name,
                Description = productAddDto.Description,
                UnitPrice = productAddDto.UnitPrice,
                UnitsInStock = productAddDto.UnitsInStock,
                CategoryId = productAddDto.CategoryId,
                ImagePath = productAddDto.ImagePath,

            };

            _productRepo.Add(entity);
        }

        public void DeleteProduct(int id)
        {
            _productRepo.Delete(id);
        }

        public List<ProductListDto> GetAllProducts()
        {
            var productEntities = _productRepo.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name);

            var productDtoList = productEntities.Select(x => new ProductListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.Name,
                ImagePath = x.ImagePath,
            }).ToList();

            return productDtoList;
        }

        public List<ProductListDto> GetAllProductsByCategoryId(int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                var productEntities = _productRepo.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);

                var productDtos = productEntities.Select(x => new ProductListDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    CategoryName = x.Category.Name,
                    ImagePath = x.ImagePath,

                }).ToList();

                return productDtos;
            }

            return GetAllProducts(); // aynı işlemler yapılacağından 
        }

        public ProductUpdateDto GetProductById(int id)
        {
            var entity = _productRepo.GetById(id);

            var dto = new ProductUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitPrice = entity.UnitPrice,
                UnitsInStock = entity.UnitsInStock,
                CategoryId = entity.CategoryId,
                ImagePath = entity.ImagePath,
            };

            return dto;
        }

        public void UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            var entity = _productRepo.GetById(productUpdateDto.Id);

            entity.Name = productUpdateDto.Name;
            entity.Description = productUpdateDto.Description;
            entity.UnitPrice = productUpdateDto.UnitPrice;  
            entity.UnitsInStock = productUpdateDto.UnitsInStock;
            entity.CategoryId = productUpdateDto.CategoryId;    

            if(productUpdateDto.ImagePath is not null)
                entity.ImagePath = productUpdateDto.ImagePath;

            _productRepo.Update(entity);
        }
    }
}
