using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PetShop.Core.ApplicationService.Impl
{
    public class ProductTypeService: IProductTypeService
    {
        private IProductTypeRepository ProductTypeRepository;

        public ProductTypeService(IProductTypeRepository productTypeRepository)
        {
            this.ProductTypeRepository = productTypeRepository;
        }

        public ProductType CreateProductType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Entered product type name too short");
            }

            return new ProductType {Name = type };
        }

        public ProductType AddProductType(ProductType type)
        {
            if (type != null)
            {
                if ((from x in GetAllProductTypes() where x.Name.ToLower().Equals(type.Name.ToLower()) select x).Count() > 0)
                {
                    throw new ArgumentException("Product type with that name already exists");
                }
                return ProductTypeRepository.AddProductType(type);
            }
            return null;
        }

        public List<ProductType> GetAllProductTypes()
        {
            return ProductTypeRepository.ReadTypes().ToList();
        }

        public List<ProductType> GetProductTypesFilterSearch(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("Page or items per page must be above zero");
            }

            IEnumerable<ProductType> types = ProductTypeRepository.ReadTypesFilterSearch(filter);

            if (filter.CurrentPage > 0 && filter.ItemsPrPage > 0)
            {
                types = types.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage).Take(filter.ItemsPrPage);
                if (types.Count() == 0)
                {
                    throw new InvalidDataException("Index out of bounds");
                }
            }

            return types.ToList();
        }

        public ProductType GetProductTypeByID(int ID)
        {
            return ProductTypeRepository.GetProductTypeByID(ID);
        }

        public ProductType UpdateProductType(ProductType type, int ID)
        {
            if (GetProductTypeByID(ID) == null)
            {
                throw new ArgumentException("No product type with such ID found");
            }
            if (type == null)
            {
                throw new ArgumentException("Updating product type does not excist");
            }
            type.ID = ID;
            return ProductTypeRepository.UpdateProductType(type);
        }

        public ProductType DeleteProductType(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetProductTypeByID(ID) == null)
            {
                throw new ArgumentException("No product type with such ID found");
            }
            return ProductTypeRepository.DeleteProductType(ID);
        }

    }
}
