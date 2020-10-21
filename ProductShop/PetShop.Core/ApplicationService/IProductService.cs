using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IProductService
    {
        Product CreateProduct(string productName, ProductType type, double price, DateTime createdDate);

        Product AddProduct(Product product);

        List<Product> GetAllProducts();

        List<Product> GetProductsFilterSearch(Filter filter);

        Product GetProductByID(int ID);

        Product UpdateProduct(Product product, int ID);

        Product DeleteProduct(int ID);

    }
}
