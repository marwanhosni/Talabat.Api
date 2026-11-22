using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.Repository;
public class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext _context)
    {

        if (_context.ProductBrands.Count() == 0)
        {

            var brandData = File.ReadAllText(path: "../Talabat.Repository/Data/DataSeed/brands.json");

            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

            if (brands.Count() > 0)
            {
                foreach (var brand in brands)
                {
                    _context.Set<ProductBrand>().Add(brand);
                }
                await _context.SaveChangesAsync();
            }
        }



        //==================================================================================================

        if (_context.ProductCategories.Count() == 0)
        {
            var CategoriesData = File.ReadAllText(path: "../Talabat.Repository/Data/DataSeed/Categories.json");

            var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

            if (Categories.Count() > 0)
            {
                foreach (var Category in Categories)
                {
                    _context.Set<ProductCategory>().Add(Category);
                }
                await _context.SaveChangesAsync();
            }
        }

        //===================================================================================================

        if (_context.products.Count() == 0)
        {
            //Reed Data From json File
            var ProductData = File.ReadAllText(path: "../Talabat.Repository/Data/DataSeed/Products.json");

            var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);

            if (Products.Count() > 0)
            {
                foreach (var product in Products)
                {
                    _context.Set<Product>().Add(product);
                }
                await _context.SaveChangesAsync();
            }
        }
        //===================================================================================================
        //DataSeed DeleviryMethods

        if (_context.DeliveryMethods.Count() == 0)

        {

            var deleviryData = File.ReadAllText(path: "../Talabat.Repository/Data/DataSeed/delivery.json");

            var deleviryMethods = JsonSerializer.Deserialize<List<DeliveryMethods>>(deleviryData);

            if (deleviryMethods.Count() > 0)
            {
                foreach (var delivery in deleviryMethods)
                {
                    _context.DeliveryMethods.Add(delivery);
                }
                await _context.SaveChangesAsync();
            }
        }


    }
}
