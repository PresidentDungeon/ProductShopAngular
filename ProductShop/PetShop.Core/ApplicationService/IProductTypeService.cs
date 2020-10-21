using PetShop.Core.Entities;
using System.Collections.Generic;

namespace PetShop.Core.ApplicationService
{
    public interface IProductTypeService
    {
        ProductType CreateProductType(string type);

        ProductType AddProductType(ProductType productType);

        List<ProductType> GetAllProductTypes();

        List<ProductType> GetProductTypesFilterSearch(Filter filter);

        ProductType GetProductTypeByID(int ID);

        ProductType UpdateProductType(ProductType type, int ID);

        ProductType DeleteProductType(int ID);
    }
}
