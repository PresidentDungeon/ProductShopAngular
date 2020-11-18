using PetShop.Core.Entities;
using ProductShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IColorRepository
    {
        Color AddColor(Color color);
        IEnumerable<Color> ReadColors();
        IEnumerable<Color> ReadColorsFilterSearch(Filter filter);
        FilterList<Color> ReadColorsFilterSearchList(Filter filter);
        Color GetColorByID(int ID);
        Color UpdateColor(Color color);
        Color DeleteColor(int ID);
    }
}
