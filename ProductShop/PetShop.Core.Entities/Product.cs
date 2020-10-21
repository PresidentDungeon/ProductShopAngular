using System;
using System.Collections.Generic;

namespace PetShop.Core.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}