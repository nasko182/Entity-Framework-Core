namespace ProductShop;

using Newtonsoft.Json;
using Data;
using DTOs.Import;
using Models;
using Microsoft.EntityFrameworkCore;
using ProductShop.DTOs.Export;

public class StartUp
{
    public static void Main()
    {
        using (var context = new ProductShopContext())
        {
            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            Console.WriteLine(GetUsersWithProducts(context));
        };
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        var usersDTOs = JsonConvert.DeserializeObject<List<UserDTO>>(inputJson);

        var users = new HashSet<User>();
        foreach (var userDTO in usersDTOs!)
        {
            User user = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Age = userDTO.Age
            };
            users.Add(user);
        }

        context.Users.AddRange(users);
        context.SaveChanges();

        return $"Successfully imported {usersDTOs.Count}";
    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        var productsDTOs = JsonConvert.DeserializeObject<List<ProductDTO>>(inputJson);
        var products = new HashSet<Product>();
        foreach (var productDTO in productsDTOs!)
        {
            var product = new Product()
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                SellerId = productDTO.SellerId,
                BuyerId = productDTO.BuyerId
            };

            products.Add(product);
        }

        context.Products.AddRange(products);
        context.SaveChanges();
        return $"Successfully imported {products.Count}";
    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        var categoriesDTOs = JsonConvert.DeserializeObject<CategoryDTO[]>(inputJson);

        var categories = new HashSet<Category>();

        foreach (var categoryDTO in categoriesDTOs!)
        {
            if (categoryDTO.Name != null)
            {
                categories.Add(new Category()
                {
                    Name = categoryDTO.Name
                });
            }
        }

        context.Categories.AddRange(categories);
        context.SaveChanges();

        return $"Successfully imported {categories.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        var categoryProductDTOs = JsonConvert.DeserializeObject<CategoryProductDTO[]>(inputJson);

        var categoryProducts = new HashSet<CategoryProduct>();

        foreach (var categoryProductDTO in categoryProductDTOs!)
        {
            if (!context.Categories.Any(c => c.Id == categoryProductDTO.CategoryId) ||
                !context.Products.Any(p => p.Id == categoryProductDTO.ProductId))
            {
                continue;
            }
            categoryProducts.Add(new CategoryProduct()
            {
                CategoryId = categoryProductDTO.CategoryId,
                ProductId = categoryProductDTO.ProductId
            });

        }

        context.CategoriesProducts.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count}";
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        var products = context.Products
            .Include(p => p.Seller)
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new ExportProductDTO(p));
        string productsInRangeJSON = JsonConvert.SerializeObject(products, Formatting.Indented);

        return productsInRangeJSON;
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        var soldProducts = context.Users
            .Include(u => u.ProductsSold)
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                soldProducts = u.ProductsSold.Select(ps => new
                {
                    name = ps.Name,
                    price = ps.Price,
                    buyerFirstName = ps.Buyer.FirstName,
                    buyerLastName = ps.Buyer.LastName,
                })
            });

        return JsonConvert.SerializeObject(soldProducts, Formatting.Indented);
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        var categoriesByProducts = context.Categories
            .OrderByDescending(c => c.CategoriesProducts
                .Where(p => p.Category.Name == c.Name).Count())
            .Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoriesProducts
                .Where(cp => cp.Category.Name == c.Name)
                .Count(),
                averagePrice = String.Format("{0:0.00}", c.CategoriesProducts.Where(cp => cp.Category.Name == c.Name).Average(p => p.Product.Price)),
                totalRevenue = String.Format("{0:0.00}", c.CategoriesProducts
                .Where(cp => cp.Category.Name == c.Name)
                .Sum(p => p.Product.Price))
            });

        return JsonConvert.SerializeObject(categoriesByProducts, Formatting.Indented);
        //return categoriesByProducts.ToString();

    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var usersWithBuyer = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderByDescending(u => u.ProductsSold.Where(p => p.Buyer != null).Count())
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold.Count(p => p.Buyer != null),
                    products = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    })
                }
            });

        var users = new
        {
            usersCount = usersWithBuyer.Count(),
            users = usersWithBuyer
        };

        return JsonConvert.SerializeObject(users,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                NullValueHandling= NullValueHandling.Ignore
            });
    }
}