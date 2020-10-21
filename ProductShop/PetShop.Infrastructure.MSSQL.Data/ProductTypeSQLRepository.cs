using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ProductTypeSQLRepository : IProductTypeRepository
    {
        private ProductShopContext ctx;

        public ProductTypeSQLRepository(ProductShopContext ctx)
        {
            this.ctx = ctx;
        }

        public ProductType AddProductType(ProductType type)
        {
            ctx.Attach(type).State = EntityState.Added;
            ctx.SaveChanges();
            return type;
        }
        public IEnumerable<ProductType> ReadTypes()
        {
            return ctx.ProductTypes.AsEnumerable();
        }

        public IEnumerable<ProductType> ReadTypesFilterSearch(Filter filter)
        {
            IQueryable<ProductType> types = ctx.ProductTypes.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                types = from x in types where x.Name.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                types = from x in types orderby x.Name select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                types = from x in types orderby x.Name descending select x;
            }

            return types;
        }

        public ProductType GetProductTypeByID(int ID)
        {
            return ctx.ProductTypes.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }

        public ProductType UpdateProductType(ProductType type)
        {
            ctx.Attach(type).State = EntityState.Modified;
            ctx.SaveChanges();
            return type;
        }

        public ProductType DeleteProductType(int ID)
        {
            var deletedProduct = ctx.ProductTypes.Remove(GetProductTypeByID(ID));
            ctx.SaveChanges();
            return deletedProduct.Entity;
        }
    }
}
