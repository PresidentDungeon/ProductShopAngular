using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.DomainService
{
    public interface IProductTypeRepository
    {
        ProductType AddProductType(ProductType type);
        IEnumerable<ProductType> ReadTypes();
        IEnumerable<ProductType> ReadTypesFilterSearch(Filter filter);
        ProductType GetProductTypeByID(int ID);
        ProductType UpdateProductType(ProductType type);
        ProductType DeleteProductType(int ID);
    }
}
