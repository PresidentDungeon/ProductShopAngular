using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class ProductType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
