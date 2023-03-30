using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            string importSupplierString = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            string importPartString = File.ReadAllText(@"../../../Datasets/parts.xml");
            string importCarString = File.ReadAllText(@"../../../Datasets/cars.xml");
            string importCustomerString = File.ReadAllText(@"../../../Datasets/customers.xml");
            string importSaleString = File.ReadAllText(@"../../../Datasets/sales.xml");

            //Console.WriteLine(ImportSuppliers(context, importSupplierString));
            //Console.WriteLine(ImportParts(context, importPartString));
            //Console.WriteLine(ImportCars(context, importCarString));
            //Console.WriteLine(ImportCustomers(context, importCustomerString));
            //Console.WriteLine(ImportSales(context, importSaleString));

            //Console.WriteLine(GetCarsWithDistance(context));
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            Console.WriteLine(GetTotalSalesByCustomer(context));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            XmlRootAttribute root = new XmlRootAttribute("Suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportSupplierDto[] supplierDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var dto in supplierDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }

                Supplier supplier = mapper.Map<Supplier>(dto);

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            XmlRootAttribute root = new XmlRootAttribute("Parts");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportPartDto[] partDtos = (ImportPartDto[])serializer.Deserialize(reader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (ImportPartDto dto in partDtos)
            {
                if (dto.SupplierId == null || !context.Suppliers.Any(x => x.Id == dto.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(dto);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            XmlRootAttribute root = new XmlRootAttribute("Cars");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportCarDto[] importCarDtos = (ImportCarDto[])xmlSerializer.Deserialize(reader);

            ICollection<Car> carsValided = new HashSet<Car>();

            foreach (ImportCarDto carDto in importCarDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make) || string.IsNullOrEmpty(carDto.Model))
                {
                    continue;
                }
                Car car = mapper.Map<Car>(carDto);

                foreach (ImportCarPartDto importPartCarDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!context.Parts.Any(p => p.Id == importPartCarDto.PartId))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        CarId = car.Id,
                        PartId = importPartCarDto.PartId
                    };

                    car.PartsCars.Add(partCar);
                }
                carsValided.Add(car);
            }

            context.Cars.AddRange(carsValided);
            context.SaveChanges();

            return $"Successfully imported {carsValided.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            XmlRootAttribute root = new XmlRootAttribute("Customers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportCustomerDto[] importCustomerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);

            ICollection<Customer> customerrsValided = new HashSet<Customer>();

            foreach (ImportCustomerDto customerDto in importCustomerDtos)
            {
                Customer customer = mapper.Map<Customer>(customerDto);
                customerrsValided.Add(customer);
            }

            context.Customers.AddRange(customerrsValided);
            context.SaveChanges();

            return $"Successfully imported {customerrsValided.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            XmlRootAttribute root = new XmlRootAttribute("Sales");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportSaleDto[] importSaleDtos = (ImportSaleDto[])xmlSerializer.Deserialize(reader);

            ICollection<Sale> salesValided = new HashSet<Sale>();

            foreach (ImportSaleDto saleDto in importSaleDtos)
            {
                if (!context.Cars.Any(c => c.Id == saleDto.CarId))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(saleDto);

                salesValided.Add(sale);
            }

            context.Sales.AddRange(salesValided);
            context.SaveChanges();

            return $"Successfully imported {salesValided.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportCarDto[] exportCarDtos = context.Cars.Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, exportCarDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportCarBmwDto[] exportBMWCarDtos = context.Cars.Where(c => c.Make.ToUpper() == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportCarBmwDto>(mapper.ConfigurationProvider).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarBmwDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, exportBMWCarDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportLocalSupplierDto[] localDto = context.Suppliers.Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSupplierDto { Id = s.Id, Name = s.Name, Count = s.Parts.Count }).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportLocalSupplierDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, localDto, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportCarWithPartsDto[] exportCarWithPartsDtos = context.Cars.OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarWithPartsDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, exportCarWithPartsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            StringBuilder sb = new StringBuilder();

            ExportCarPriceDto[] carPriceDtos = context.Customers
                .Where(c => c.Sales.Count > 0)
                .ProjectTo<ExportCarPriceDto>(mapper.ConfigurationProvider)
                .ToArray();

            CustomersWithSalesDto[] customersWithSalesDtos =
                mapper.Map<CustomersWithSalesDto[]>(carPriceDtos)
                .OrderByDescending(c => c.MoneySpent)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomersWithSalesDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, customersWithSalesDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}