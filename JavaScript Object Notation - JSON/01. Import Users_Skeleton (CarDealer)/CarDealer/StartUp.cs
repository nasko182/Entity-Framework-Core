using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        using (var context = new CarDealerContext())
        {
            //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        };
    }

    public static string ImportSuppliers(CarDealerContext context, string inputJson)
    {
        var suppliersDTOs = JsonConvert.DeserializeObject<ImportSupplierDTO[]>(inputJson);

        var suppliers = new List<Supplier>();
        foreach (var supplierDTO in suppliersDTOs!)
        {
            suppliers.Add(new Supplier()
            {
                Name = supplierDTO.Name,
                IsImporter = supplierDTO.IsImporter
            });
        }
        //context.Suppliers.AddRange(suppliers);
        //context.SaveChanges();

        return $"Successfully imported {suppliers.Count}.";
    }

    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        var partsDTOs = JsonConvert.DeserializeObject<importPartDTO[]>(inputJson);

        var parts = new List<Part>();

        foreach (var partDTO in partsDTOs!)
        {
            if (context.Suppliers.Any(s => s.Id == partDTO.SupplierId))
            {
                parts.Add(new Part()
                {
                    Name = partDTO.Name,
                    Price = partDTO.Price,
                    Quantity = partDTO.Quantity,
                    SupplierId = partDTO.SupplierId
                });
            }
        }

        //context.Parts.AddRange(parts);
        //context.SaveChanges();

        return $"Successfully imported {parts.Count}.";
    }

    public static string ImportCars(CarDealerContext context, string inputJson)
    {
        var carsDTO = JsonConvert.DeserializeObject<ImportCarDTO[]>(inputJson);

        var parts = new HashSet<Part>();

        var cars = new List<Car>();
        foreach (var carDTO in carsDTO!)
        {
            Car car = new Car()
            {
                Make = carDTO.Make,
                Model = carDTO.Model,
                TraveledDistance = carDTO.TravelledDistance
            };

            foreach (var partId in carDTO.PartsId)
            {
                if (context.Parts.Any(p => p.Id == partId)
                    && car.PartsCars.FirstOrDefault(pc => pc.PartId == partId) == null)
                {
                    car.PartsCars.Add(new PartCar()
                    {
                        PartId = partId
                    });
                }
            }

            cars.Add(car);
        }

        //context.Cars.AddRange(cars);
        //context.SaveChanges();

        return $"Successfully imported {cars.Count}.";
    }

    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        var customersDTOs = JsonConvert.DeserializeObject<ImportCustomerDTO[]>(inputJson);

        var customers = new List<Customer>();
        foreach (var customerDTO in customersDTOs!)
        {
            customers.Add(new Customer(customerDTO));
        }

        context.Customers.AddRange(customers);
        context.SaveChanges();

        return $"Successfully imported {customers.Count}.";

    }

    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        var salesDTOs = JsonConvert.DeserializeObject<ImportSaleDTO[]>(inputJson);

        var sales = new HashSet<Sale>();
        foreach (var saleDTO in salesDTOs!)
        {
            sales.Add(new Sale(saleDTO));
        }

        context.Sales.AddRange(sales);
        context.SaveChanges();

        return $"Successfully imported {sales.Count}.";
    }

    public static string GetOrderedCustomers(CarDealerContext context)
    {
        var customers = context.Customers
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .Select(c => new
            {
                c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy", new CultureInfo("es-ES")),
                c.IsYoungDriver
            });

        return JsonConvert.SerializeObject(customers, Formatting.Indented);
    }

    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        var cars = context.Cars
            .Where(c => c.Make == "Toyota")
            .Select(c => new
            {
                c.Id,
                c.Make,
                c.Model,
                c.TraveledDistance,
            })
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance);

        return JsonConvert.SerializeObject(cars, Formatting.Indented);

    }

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        var localSuppliers = context.Suppliers
            .Where(s => !s.IsImporter)
            .Select(s => new
            {
                s.Id,
                s.Name,
                PartsCount = s.Parts.Count()
            });

        return JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        var cars = context.Cars
            .Include(c => c.PartsCars)
            .Select(c => new
            {
                car = new
                {
                    c.Make,
                    c.Model,
                    c.TraveledDistance
                },
                parts = c.PartsCars.Select(p => new
                {
                    Name = p.Part.Name,
                    Price = $"{p.Part.Price:f2}"
                })
            });

        return JsonConvert.SerializeObject(cars, Formatting.Indented);
    }

    //TODO: 50/100
    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        var customers = context.Customers
            .Include(c => c.Sales)
            .ThenInclude(c => c.Car)
            .ThenInclude(c => c.PartsCars)
            .ThenInclude(c => c.Part)
            .Where(c => c.Sales.Count >= 1)
            .ToArray()
            .Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count,
                spentMoney = Math.Round(c.Sales.Sum(s => s.Car.PartsCars.Sum(pc => pc.Part.Price)), 2)
            })
            .OrderByDescending(c => c.spentMoney)
            .ThenByDescending(c => c.boughtCars);

        return JsonConvert.SerializeObject(customers, Formatting.Indented);
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        var sales = context.Sales.Take(10)
               .Select(s => new
               {
                   car = new
                   {
                       Make = s.Car.Make,
                       Model = s.Car.Model,
                       TraveledDistance = s.Car.TraveledDistance

                   },
                   customerName = s.Customer.Name,
                   discount = s.Discount.ToString("F2"),
                   price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("F2"),
                   priceWithDiscount = (s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - (s.Discount / 100))).ToString("F2")
               });

        return JsonConvert.SerializeObject(sales, Formatting.Indented);
    }
}