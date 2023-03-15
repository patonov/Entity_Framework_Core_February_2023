using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {        
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            

            string inputUserString = File.ReadAllText(@"../../../Datasets/users.json");
            string inputProductString = File.ReadAllText(@"../../../Datasets/products.json");

            //Console.WriteLine(ImportUsers(context, inputUserString));
            Console.WriteLine(ImportProducts(context, inputProductString));

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            ICollection<User> usersValided = new HashSet<User>();

            foreach (ImportUserDto userdto in userDtos)
            { 
            User user = mapper.Map<User>(userdto);
                usersValided.Add(user);
            }

            context.Users.AddRange(usersValided);

            context.SaveChanges();

            return $"Successfully imported {usersValided.Count}"; 
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<ProductShopProfile>();            
            }));

            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            ICollection<Product> productsValided = new HashSet<Product>();

            foreach (ImportProductDto dto in productDtos)
            {
                Product product = mapper.Map<Product>(dto);
                productsValided.Add(product);
            }

            context.Products.AddRange(productsValided);
            context.SaveChanges();

            return $"Successfully imported {productsValided.Count}";

        }



    }
}