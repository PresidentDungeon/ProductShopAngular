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
        private IColorRepository ColorRepository;
        private IUserService UserService;

        public InitStaticData(IProductRepository productRepository, IProductTypeRepository typeRepository, IColorRepository colorRepository, IUserService userService)
        {
            this.ProductRepository = productRepository;
            this.TypeRepository = typeRepository;
            this.ColorRepository = colorRepository;
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

            Color red = new Color { ColorDescription = "Red" };
            Color blue = new Color { ColorDescription = "Blue" };

            ColorRepository.AddColor(red);
            ColorRepository.AddColor(blue);

            Color green = new Color { ColorDescription = "Green" };
            Color yellow = new Color { ColorDescription = "Yellow" };
            Color orange = new Color { ColorDescription = "Orange" };
            Color purple = new Color { ColorDescription = "Purple" };
            Color caramel = new Color { ColorDescription = "Caramel" };
            Color green1 = new Color { ColorDescription = "Green" };
            Color yellow1 = new Color { ColorDescription = "Yellow" };
            Color orange1 = new Color { ColorDescription = "Orange" };
            Color purple1 = new Color { ColorDescription = "Purple" };
            Color caramel1 = new Color { ColorDescription = "Caramel" };
            Color green2 = new Color { ColorDescription = "Green" };
            Color yellow2 = new Color { ColorDescription = "Yellow" };
            Color orange2 = new Color { ColorDescription = "Orange" };
            Color purple2 = new Color { ColorDescription = "Purple" };
            Color caramel2 = new Color { ColorDescription = "Caramel" };
            Color green3 = new Color { ColorDescription = "Green" };
            Color yellow3 = new Color { ColorDescription = "Yellow" };
            Color orange3 = new Color { ColorDescription = "Orange" };
            Color purple3 = new Color { ColorDescription = "Purple" };
            Color caramel3 = new Color { ColorDescription = "Caramel" };
            Color green4 = new Color { ColorDescription = "Green" };
            Color yellow4 = new Color { ColorDescription = "Yellow" };
            Color orange4 = new Color { ColorDescription = "Orange" };
            Color purple4 = new Color { ColorDescription = "Purple" };
            Color caramel4 = new Color { ColorDescription = "Caramel" };
            Color green5 = new Color { ColorDescription = "Green" };
            Color yellow5 = new Color { ColorDescription = "Yellow" };
            Color orange5 = new Color { ColorDescription = "Orange" };
            Color purple5 = new Color { ColorDescription = "Purple" };
            Color caramel5 = new Color { ColorDescription = "Caramel" };

            ColorRepository.AddColor(green);
            ColorRepository.AddColor(yellow);
            ColorRepository.AddColor(orange);
            ColorRepository.AddColor(purple);
            ColorRepository.AddColor(caramel);
            ColorRepository.AddColor(green1);
            ColorRepository.AddColor(yellow1);
            ColorRepository.AddColor(orange1);
            ColorRepository.AddColor(purple1);
            ColorRepository.AddColor(caramel1);
            ColorRepository.AddColor(green2);
            ColorRepository.AddColor(yellow2);
            ColorRepository.AddColor(orange2);
            ColorRepository.AddColor(purple2);
            ColorRepository.AddColor(caramel2);
            ColorRepository.AddColor(green3);
            ColorRepository.AddColor(yellow3);
            ColorRepository.AddColor(orange3);
            ColorRepository.AddColor(purple3);
            ColorRepository.AddColor(caramel3);
            ColorRepository.AddColor(green4);
            ColorRepository.AddColor(yellow4);
            ColorRepository.AddColor(orange4);
            ColorRepository.AddColor(purple4);
            ColorRepository.AddColor(caramel4);
            ColorRepository.AddColor(green5);
            ColorRepository.AddColor(yellow5);
            ColorRepository.AddColor(orange5);
            ColorRepository.AddColor(purple5);
            ColorRepository.AddColor(caramel5);

            ProductRepository.AddProduct(new Product
            {
                Name = "Super Hammer",
                Type = tools,
                Price = 750.0,
                productColors = new List<ProductColor> { new ProductColor { Color = blue }, new ProductColor { Color = red } },
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
