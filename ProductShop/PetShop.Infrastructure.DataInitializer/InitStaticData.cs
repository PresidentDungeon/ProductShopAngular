using PetShop.Core.ApplicationService;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PetShop.Infrastructure.Data
{
    public class InitStaticData
    {
        private IProductRepository ProductRepository;
        private IProductTypeRepository TypeRepository;
        private IUserService UserService;

        public InitStaticData(IProductRepository productRepository, IProductTypeRepository typeRepository, IUserService userService)
        {
            this.ProductRepository = productRepository;
            this.TypeRepository = typeRepository;
            this.UserService = userService;
        }
        public void InitData()
        {
            ProductType tools = new ProductType{Name = "Tools"};
            ProductType food = new ProductType{Name = "Food"};
            ProductType wearable = new ProductType{Name = "Wearable"};

            TypeRepository.AddProductType(tools);
            TypeRepository.AddProductType(food);
            TypeRepository.AddProductType(wearable);

            ProductRepository.AddProduct(new Product
            {
                Name = "Super Hammer",
                Type = tools,
                Price = 750.0,
                CreatedDate = DateTime.Parse("19-10-2020 18:00", CultureInfo.GetCultureInfo("da-DK").DateTimeFormat),
            });
            ProductRepository.AddProduct(new Product
            {
                Name = "FishBone",
                Type = food,
                Price = 365.25,
                CreatedDate = DateTime.Parse("17-10-2020 15:35", CultureInfo.GetCultureInfo("da-DK").DateTimeFormat),
            });
            ProductRepository.AddProduct(new Product
            {
                Name = "ClapeHat",
                Type = wearable,
                Price = 650.0,
                CreatedDate = DateTime.Parse("19-10-2019 12:41", CultureInfo.GetCultureInfo("da-DK").DateTimeFormat),
            });

            UserService.AddUser(UserService.CreateUser("Hans", "kodeord", "Admin"));
            UserService.AddUser(UserService.CreateUser("Kurt", "lasagne28", "User"));
        }
    }
}
