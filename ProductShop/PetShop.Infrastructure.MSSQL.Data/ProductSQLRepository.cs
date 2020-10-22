﻿using Microsoft.EntityFrameworkCore;
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
            return ctx.Products.Include(product => product.Type).Include(p => p.productColors).ThenInclude(c => c.Color).AsEnumerable();
        }

        public IEnumerable<Product> ReadProductsFilterSearch(Filter filter)
        {

            IQueryable<Product> products = ctx.Products.Include(product => product.Type).Include(p => p.productColors).ThenInclude(c => c.Color).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = from x in products where x.Name.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.ProductType))
            {
                products = from x in products where x.Type.Name.ToLower().Equals(filter.ProductType.ToLower()) select x;
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
            return ctx.Products.AsNoTracking().Include(product => product.Type).Include(p => p.productColors)
                .ThenInclude(c => c.Color).FirstOrDefault(x => x.ID == ID);
        }

        public Product UpdateProduct(Product product)
        {
            List<ProductColor> productColors = ctx.ProductColors.AsNoTracking().ToList();

            //fjerne alle der ikke bruges
            List<ProductColor> color = productColors.Where(p => product.productColors.All(p2 => p2.ColorID != p.ColorID) && p.ProductID == product.ID).ToList();
            ctx.ProductColors.RemoveRange(color);

            //tilføje alle nye
            List<ProductColor> colorsToAdd = product.productColors.Where(p => productColors.All(p2 => p2.ColorID != p.ColorID || p2.ProductID != product.ID)).ToList();
            colorsToAdd.ForEach(x => x.ProductID = product.ID);
            ctx.ProductColors.AddRange(colorsToAdd);

            product.productColors = null;

            ctx.Attach(product).State = EntityState.Modified;
            ctx.Entry(product).Reference(product => product.Type).IsModified = true;
            ctx.SaveChanges();

            return GetProductByID(product.ID);
        }

        public Product DeleteProduct(int ID)
        {
            var removedProduct = ctx.Products.Remove(GetProductByID(ID));
            ctx.SaveChanges();
            return removedProduct.Entity;
        }
    }
}
