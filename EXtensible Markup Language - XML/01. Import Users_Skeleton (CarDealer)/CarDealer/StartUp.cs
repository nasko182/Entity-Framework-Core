using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new CarDealerContext())
            {
            //    string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
                Console.WriteLine(GetTotalSalesByCustomer(context));
            }
        }
        //PROLEM 1

        //PROLEM 2

        //PROLEM 3

        //PROLEM 4

        //PROLEM 5

        //PROLEM 6

        //PROLEM 7

        //PROLEM 8

        //PROLEM 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            string rootName = "Suppliers";
            XmlHelper xmlHelper = new XmlHelper();
            var suppliersDTOS = xmlHelper.Deserialize<ImportSupplierDTO[]>(inputXml, rootName);

            var suppliers = new List<Supplier>();

            foreach (var supplierDTO in suppliersDTOS)
            {
                if (string.IsNullOrEmpty(supplierDTO.Name))
                {
                    suppliers.Add(new Supplier(supplierDTO));
                }
            }

            //context.Suppliers.AddRange(suppliers);
            //context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //PROLEM 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            string rootName = "Parts";
            XmlHelper xmlHelper = new XmlHelper();

            var partsDtos = xmlHelper.Deserialize<ImportPartDTO[]>(inputXml, rootName);

            var parts = new List<Part>();
            foreach (var partDTO in partsDtos)
            {
                if (!string.IsNullOrEmpty(partDTO.Name) && context.Suppliers.Any(s => s.Id == partDTO.SupplierId))
                {
                    parts.Add(new Part(partDTO));
                }
            }

            context.AddRange(parts);
            //context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //PROLEM 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlHelper = new XmlHelper();
            string rootName = "Cars";

            var carsDTOs = xmlHelper.Deserialize<ImportCarDTO[]>(inputXml, rootName);

            var cars = new List<Car>();

            foreach (var carDto in carsDTOs!)
            {
                if (!string.IsNullOrEmpty(carDto.Make) ||
                    !string.IsNullOrEmpty(carDto.Model))
                {
                    Car car = new Car(carDto);
                    foreach (var part in carDto.Parts.DistinctBy(p => p.PartID))
                    {
                        if (context.Parts.AsNoTracking().Any(p => p.Id == part.PartID))
                        {
                            car.PartsCars.Add(new PartCar()
                            {
                                PartId = part.PartID,
                            });
                        }
                    }
                    cars.Add(car);
                }
            }

            context.AddRange(cars);
            //context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //PROLEM 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var root = new XmlRootAttribute("Customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomersDTO[]), root);
            StringReader reader = new StringReader(inputXml);

            var customersDTOs = (ImportCustomersDTO[]?)xmlSerializer.Deserialize(reader);

            var customers = new List<Customer>();
            foreach (var customerDto in customersDTOs!)
            {

                if (!string.IsNullOrEmpty(customerDto.Name))
                {
                    customers.Add(new Customer(customerDto));
                }
            }

            context.AddRange(customers);
            //context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //PROLEM 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var root = new XmlRootAttribute("Sales");
            var xmlSerializer = new XmlSerializer(typeof(ImportSaleDTO[]), root);
            var reader = new StringReader(inputXml);

            var salesDTOs = (ImportSaleDTO[]?)xmlSerializer.Deserialize(reader);

            var sales = new List<Sale>();
            foreach (var saleDTO in salesDTOs!)
            {

                if (context.Cars.AsNoTracking().Any(c=>c.Id==saleDTO.CarId))
                {
                    sales.Add(new Sale(saleDTO)); 
                }
            }

            context.AddRange(sales);
            //context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //PROLEM 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsDTOs = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarWithElementsDTO(c))
                .ToArray();

            string rootName = "cars";
            var xmlSerializer = new XmlHelper();

            return xmlSerializer.Serialize(carsDTOs, rootName);
        }
        //PROLEM 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCarsDTOs = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new ExportCarWithAttributesDTO(c))
                .ToArray();

            var xmlHelper = new XmlHelper();
            string rootName = "cars";

            return xmlHelper.Serialize(bmwCarsDTOs, rootName);
        }
        //PROLEM 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .AsNoTracking()
                .Where(s => !s.IsImporter)
                .Include(s=>s.Parts)
                .Select(s => new ExportSuppliersWithAttributesDTO(s))
                .ToArray();

            var xmlHelper = new XmlHelper();
            string rootName = "suppliers";

            return xmlHelper.Serialize(localSuppliers, rootName);
        }
        //PROLEM 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsDTOs = context.Cars
                .AsNoTracking()
                .Include(c => c.PartsCars)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TraveledDistance,
                    Parts = c.PartsCars.Select(s => new
                    {
                        s.PartId,
                        s.CarId,
                        s.Part.Price
                    })
                })
                .OrderByDescending(c=>c.TraveledDistance)
                .ThenBy(c=>c.Model)
                .Take(5)
                .ToArray();

            var carsWithParts = new List<ExportCarWithPartWithAttributesDTO>();
            foreach (var carDTO in carsDTOs)
            {
                var parts = new List<ExportCarPartsWithAttributesDTOs>();

                foreach (var part in carDTO.Parts
                    .Where(cp=>cp.CarId==carDTO.Id)
                    .Select(cp=> new
                    {
                        cp.PartId,
                        cp.Price
                    })
                    .OrderByDescending(cp=>cp.Price))
                {
                    parts.Add(new ExportCarPartsWithAttributesDTOs(context.Parts
                        .Where(p=>p.Id==part.PartId)
                        .First()));
                }

                carsWithParts.Add(new ExportCarWithPartWithAttributesDTO()
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TraveledDistance = carDTO.TraveledDistance,
                    Parts = parts.ToArray()
                });
            }
                var rootName = "cars";
            var xmlHelper = new XmlHelper();

            return xmlHelper.Serialize(carsWithParts, rootName);
        }
        //PROLEM 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .AsNoTracking()
                .Where(c => c.Sales.Any())
                .Include(c=>c.Sales)
                .ThenInclude(s=>s.Car)
                .ThenInclude(car=>car.PartsCars)
                .ThenInclude(cp=>cp.Part)
                .ToArray();

            var customersDtos = new List<ExCustomersWithAttrDTO>();
            foreach (var customer in customers)
            {
                var spentMoney = customer.Sales.Where(s => s.CustomerId == customer.Id).Sum(c => c.Car.PartsCars.Sum(cp => cp.Part.Price));

                if (customer.IsYoungDriver)
                {
                    spentMoney *= 0.95M;
                }

                customersDtos.Add(new ExCustomersWithAttrDTO(customer, $"{spentMoney:f2}"));
            }
            var rootName = "customers";
            var xmlHelper = new XmlHelper();

            var res = customersDtos.OrderByDescending(c => decimal.Parse(c.SpentMoney)).ToList();

            return xmlHelper.Serialize(res, rootName);
        }

        //PROLEM 19
    }

}