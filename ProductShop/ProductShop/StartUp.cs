using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            string inputCateroryString = File.ReadAllText(@"../../../Datasets/categories.json");
            string inputCateroryProductsString = File.ReadAllText(@"../../../Datasets/categories-products.json");

            //Console.WriteLine(ImportUsers(context, inputUserString));
            //Console.WriteLine(ImportProducts(context, inputProductString));
            //Console.WriteLine(ImportCategories(context, inputCateroryString));
            //Console.WriteLine(ImportCategoryProducts(context, inputCateroryProductsString));

            //Console.Write(GetProductsInRange(context));
            Console.WriteLine(GetSoldProducts(context));
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

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            ImportCategoriesDto[] categoriesDtos = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);
            ICollection<Category> categoriesValided = new HashSet<Category>();

            foreach (ImportCategoriesDto dto in categoriesDtos)
            {
                if (dto.Name != null)
                {
                    Category category = mapper.Map<Category>(dto);
                    categoriesValided.Add(category);
                }
            }

            context.Categories.AddRange(categoriesValided);
            context.SaveChanges();

            return $"Successfully imported {categoriesValided.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            ImportCategoryProductDto[] dtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);
            ICollection<CategoryProduct> cpValided = new HashSet<CategoryProduct>();

            foreach (ImportCategoryProductDto dto in dtos)
            {
                //if (!context.Categories.Any(c => c.Id == dto.CategoryId) || !context.Products.Any(p => p.Id == dto.ProductId))
                // {
                //  continue;
                // }

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(dto);
                cpValided.Add(categoryProduct);
            }

            context.CategoriesProducts.AddRange(cpValided);
            context.SaveChanges();

            return $"Successfully imported {cpValided.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000).OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        seller = p.Seller.FirstName + " " + p.Seller.LastName
                    })
                    .AsNoTracking().ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };

            var usersWithProductsSold = context.Users.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold.Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    }).ToArray()
                }).AsNoTracking().ToArray();

            return JsonConvert.SerializeObject(usersWithProductsSold, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
        
        
        }



    }
   
}