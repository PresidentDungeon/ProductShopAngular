using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ProductSQLRepository : IProductRepository
    {
        private ProductShopContext ctx;

        public ProductSQLRepository(ProductShopContext ctx)
        {
            this.ctx = ctx;
        }

        public Product AddProduct(Product product)
        {
            ctx.Attach(product).State = EntityState.Added;
            ctx.SaveChanges();

            return product;
        }

        public IEnumerable<Product> ReadProducts()
        {
            return ctx.Products.Include(product => product.Type).AsEnumerable();
        }

        public IEnumerable<Product> ReadProductsFilterSearch(Filter filter)
        {

            IQueryable<Product> products = ctx.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = from x in products where x.Name.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.ProductType))
            {
                products = from x in products where x.Type.ToLower().Equals(filter.ProductType.ToLower()) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                //ctx.Pets.Where(p => p.Owner == null).OrderBy(p => p.Price);
                products = from x in products orderby x.Price select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                //ctx.Pets.Where(p => p.Owner == null).OrderByDescending(p => p.Price);
                products = from x in products orderby x.Price descending select x;
            }

            return products;
        }

        public Product GetProductByID(int ID)
        {
            return ctx.Products.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }

        public Product UpdateProduct(Product product)
        {
            ctx.Attach(product).State = EntityState.Modified;
            ctx.SaveChanges();
            return product;
        }

        public Product DeleteProduct(int ID)
        {
            var removedProduct = ctx.Products.Remove(GetProductByID(ID));
            ctx.SaveChanges();
            return removedProduct.Entity;
        }
    }
}
