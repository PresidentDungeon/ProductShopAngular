using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PetShop.Core.ApplicationService.Impl
{
    public class ProductService : IProductService
    {
        private IProductRepository ProductRepository;
        private IProductTypeRepository ProductTypeRepository;
        private IColorRepository ColorRepository;

        public ProductService(IProductRepository productRepository, IProductTypeRepository productTypeRepository, IColorRepository colorRepository)
        {
            this.ProductRepository = productRepository;
            this.ProductTypeRepository = productTypeRepository;
            this.ColorRepository = colorRepository;
        }

        public Product CreateProduct(string productName, ProductType type, double price, List<ProductColor> productColors, DateTime CreatedDate)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException("Entered product name too short");
            }
            if(price < 0)
            {
                throw new ArgumentException("Product price can't be negative");
            }
            if(type == null)
            {
                throw new ArgumentException("The type of product is invalid");
            }
            else
            {
                if (ProductTypeRepository.ReadTypes().Where((x) => { return x.ID == type.ID; }).FirstOrDefault() == null)
                {
                    throw new ArgumentException("No product type with such an ID found");
                }
            }
            if (productColors == null || productColors.Count == 0)
            {
                throw new ArgumentException("Entered color description too short");
            }

            if ((DateTime.Now.Year - CreatedDate.Year) > 150 || (DateTime.Now.Year - CreatedDate.Year) < 0)
            {
                throw new ArgumentException("Invalid creation date selected");
            }

            foreach (ProductColor color in productColors)
            {
                if (ColorRepository.GetColorByID(color.ColorID) == null)
                {
                    throw new ArgumentException("Invalid color");
                }
            }

            return new Product { Name = productName, Type = type, Price = price, productColors = productColors, CreatedDate = CreatedDate };
        }

        public Product AddProduct(Product product)
        {
            if(product != null)
            {
                return ProductRepository.AddProduct(product);
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            return ProductRepository.ReadProducts().ToList();
        }

        public List<Product> GetProductsFilterSearch(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("Page or items per page must be above zero");
            }

            IEnumerable<Product> products = ProductRepository.ReadProductsFilterSearch(filter);

            if (filter.CurrentPage > 0 && filter.ItemsPrPage > 0)
            {
                products = products.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage).Take(filter.ItemsPrPage);
                if (products.Count() == 0)
                {
                    throw new InvalidDataException("Index out of bounds");
                }
            }

            return products.ToList();
        }

        public Product GetProductByID(int ID)
        {
            return ProductRepository.GetProductByID(ID);
        }

        public Product UpdateProduct(Product product, int ID)
        {
            if (GetProductByID(ID) == null)
            {
                throw new ArgumentException("No product with such ID found");
            }
            if(product == null)
            {
                throw new ArgumentException("Updating product does not exist");
            }
            product.ID = ID;
            return ProductRepository.UpdateProduct(product);
        }

        public Product DeleteProduct(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Incorrect ID entered");
            }
            if (GetProductByID(ID) == null)
            {
                throw new ArgumentException("No product with such ID found");
            }
            return ProductRepository.DeleteProduct(ID);
        }
    }
}
