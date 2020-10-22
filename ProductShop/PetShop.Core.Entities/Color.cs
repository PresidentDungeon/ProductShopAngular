using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class Color
    {
        public int ID { get; set; }
        public string ColorDescription { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}
