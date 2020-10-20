using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IProductRepository
    {
        Product AddProduct(Product product);
        IEnumerable<Product> ReadProducts();
        IEnumerable<Product> ReadProductsFilterSearch(Filter filter);
        Product GetProductByID(int ID);
        Product UpdateProduct(Product product);
        Product DeleteProduct(int ID);
    }
}
