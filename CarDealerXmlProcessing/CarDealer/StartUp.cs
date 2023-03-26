﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Reflection.Metadata.Ecma335;
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

            //Console.WriteLine(ImportSuppliers(context, importSupplierString));
            //Console.WriteLine(ImportParts(context, importPartString));
            Console.WriteLine(ImportCars(context, importCarString));
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
                if(string.IsNullOrEmpty(carDto.Make) || string.IsNullOrEmpty(carDto.Model))
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


    }
}