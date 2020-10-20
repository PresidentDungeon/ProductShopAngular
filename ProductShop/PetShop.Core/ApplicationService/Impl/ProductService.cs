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

        public ProductService(IProductRepository productRepository)
        {
            this.ProductRepository = productRepository;
        }

        public Product CreateProduct(string productName, string type, double price, DateTime CreatedDate)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentException("Entered product name too short");
            }
            if(price < 0)
            {
                throw new ArgumentException("Product price can't be negative");
            }
            if(string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("The type of product is invalid");
            }
            if ((DateTime.Now.Year - CreatedDate.Year) > 150 || (DateTime.Now.Year - CreatedDate.Year) < 0)
            {
                throw new ArgumentException("Invalid creation date selected");
            }
            return new Product { Name = productName, Type = type, Price = price, CreatedDate = CreatedDate };
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
                throw new ArgumentException("Updating product does not excist");
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
