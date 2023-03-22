using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            string inputSupplierString = File.ReadAllText(@"../../../Datasets/suppliers.json");
            string inputPartString = File.ReadAllText(@"../../../Datasets/parts.json");
            string inputCarString = File.ReadAllText(@"../../../Datasets/cars.json");
            string inputCustomerString = File.ReadAllText(@"../../../Datasets/customers.json");
            string inputSaleString = File.ReadAllText(@"../../../Datasets/sales.json");

            //Console.WriteLine(ImportSuppliers(context, inputSupplierString));
            //Console.WriteLine(ImportParts(context, inputPartString));
            //Console.WriteLine(ImportCars(context, inputCarString));
            //Console.WriteLine(ImportCustomers(context, inputCustomerString));
            //Console.WriteLine(ImportSales(context, inputSaleString));

            //Console.WriteLine(GetOrderedCustomers(context));
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            Console.WriteLine(GetTotalSalesByCustomer(context));
        
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            ImportSuppliersDto[] suppliersDtos = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);
            ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

            foreach (ImportSuppliersDto dto in suppliersDtos)
            {
                Supplier supplier = mapper.Map<Supplier>(dto);
                validSuppliers.Add(supplier);            
            }

            context.Suppliers.AddRange(validSuppliers);

            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}.";

        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            ImportPartsDto[] partsDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);
            ICollection<Part> validParts = new HashSet<Part>();

            foreach (ImportPartsDto dto in partsDtos)
            {
                Part part = mapper.Map<Part>(dto);
                if (part.SupplierId <= 0 || part.SupplierId > 31)
                {
                    continue;
                }
            validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            ImportCarsDto[] carsDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);
            ICollection<Car> carsValid = new HashSet<Car>();

            foreach (var dto in carsDtos)
            {
                var car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TravelledDistance
                };

                foreach (var partId in dto.PartsCars.Distinct())
                {
                    car.PartsCars.Add(new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    });
                }

                carsValid.Add(car);
            }

            context.Cars.AddRange(carsValid);
            context.SaveChanges();

            return $"Successfully imported {carsValid.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            ImportCustomersDto[] customersDtos = JsonConvert.DeserializeObject<ImportCustomersDto[]>(inputJson);
            ICollection<Customer> customersValided = new HashSet<Customer>();

            foreach (ImportCustomersDto dto in customersDtos)
            {
                Customer customer = mapper.Map<Customer>(dto);
            customersValided.Add(customer);
            }

            context.Customers.AddRange(customersValided);
            context.SaveChanges();

            return $"Successfully imported {customersValided.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            ImportSalesDto[] salesDtos = JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);
            ICollection<Sale> validSales = new HashSet<Sale>();

            foreach (ImportSalesDto dto in salesDtos)
            {
                Sale sale = mapper.Map<Sale>(dto);
                validSales.Add(sale);
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        { 
        var customers = context.Customers.OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                .Select(c => new 
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                }).AsNoTracking().ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            //IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            // {
            //    cfg.AddProfile<CarDealerProfile>();
            // }));

            //  var carsToExpooo = context.Cars.Where(c => c.Make == "Toyota").OrderBy(c => c.Model).ThenByDescending(c => c.TravelledDistance)
            //    .ProjectTo<ExportToyotaMakeDto>(mapper.ConfigurationProvider);

            // var cars = mapper.Map<IEnumerable<ExportToyotaMakeDto>>(carsToExpooo);

            // return JsonConvert.SerializeObject(cars, Formatting.Indented);

            var carsToExpooo = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    TraveledDistance = c.TravelledDistance,
                 }).AsNoTracking().ToArray()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance);
                        
            return JsonConvert.SerializeObject(carsToExpooo, Formatting.Indented);
        }


        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers.Where(s => s.IsImporter == false).Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count()
            }).AsNoTracking().ToList();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {

            var cars = context.Cars.Select(c => new
            {
                car = new { Make = c.Make, Model = c.Model, TraveledDistance = c.TravelledDistance },
                parts = c.PartsCars.Select(p => new { Name = p.Part.Name, Price = p.Part.Price })
            }).ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(c => c.Sales.Count() > 0).Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    cars = c.Sales
                        .Select(s => new
                        {
                            carModelName = s.Car.Model,
                            spending = s.Car.PartsCars.Sum(pc => pc.Part.Price)
                        }).ToArray(),
                }).ToArray(); 


            var customersToExport = customers.Select(c => new
                {
                    fullName = c.fullName,
                    boughtCars = c.boughtCars,
                    spentMoney = c.cars.Sum(c => c.spending)
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars).ToArray();

            return JsonConvert.SerializeObject(customersToExport, Formatting.Indented);
        }


    }
}