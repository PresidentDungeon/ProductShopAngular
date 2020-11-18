using Microsoft.EntityFrameworkCore;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using ProductShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ColorSQLRepository : IColorRepository
    {
        private ProductShopContext ctx;

        public ColorSQLRepository(ProductShopContext ctx)
        {
            this.ctx = ctx;
        }

        public Color AddColor(Color color)
        {
            ctx.Attach(color).State = EntityState.Added;
            ctx.SaveChanges();
            return color;
        }
        public IEnumerable<Color> ReadColors()
        {
            return ctx.Colors.AsEnumerable();
        }
        public IEnumerable<Color> ReadColorsFilterSearch(Filter filter)
        {
            IQueryable<Color> colors = ctx.Colors.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                colors = from x in colors where x.ColorDescription.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                colors = from x in colors orderby x.ColorDescription select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                colors = from x in colors orderby x.ColorDescription descending select x;
            }

            return colors;
        }

        public FilterList<Color> ReadColorsFilterSearchList(Filter filter)
        {
            IQueryable<Color> colors = ctx.Colors.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                colors = from x in colors where x.ColorDescription.Contains(filter.Name) select x;
            }
            if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("asc"))
            {
                colors = from x in colors orderby x.ColorDescription select x;
            }
            else if (!string.IsNullOrEmpty(filter.Sorting) && filter.Sorting.ToLower().Equals("desc"))
            {
                colors = from x in colors orderby x.ColorDescription descending select x;
            }

            int totalItems = colors.Count();

            if (filter.CurrentPage > 1)
            {
                colors = colors.Skip((filter.CurrentPage - 1) * filter.ItemsPrPage).Take(filter.ItemsPrPage);
                if (colors.Count() == 0)
                {
                    throw new InvalidDataException("Index out of bounds");
                }
            }

            FilterList<Color> filterList = new FilterList<Color> { totalItems = totalItems, List = colors.ToList() };

            return filterList;
        }

        public Color GetColorByID(int ID)
        {
            return ctx.Colors.AsNoTracking().FirstOrDefault(x => x.ID == ID);
        }
        public Color UpdateColor(Color color)
        {
            ctx.Attach(color).State = EntityState.Modified;
            ctx.SaveChanges();
            return color;
        }
        public Color DeleteColor(int ID)
        {
            var deletedColor = ctx.Colors.Remove(GetColorByID(ID));
            ctx.SaveChanges();
            return deletedColor.Entity;
        }
    }
}
