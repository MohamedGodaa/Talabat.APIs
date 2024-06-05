using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbContext.SaveChangesAsync(); 
                }
            }
            
            
            if (_dbContext.ProductCategories.Count() == 0)
            {
                var CategorieData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var Categories = JsonSerializer.Deserialize<List<Productcategories>>(CategorieData);
                if (Categories?.Count > 0)
                {
                    foreach (var Categorie in Categories)
                    {
                        _dbContext.Set<Productcategories>().Add(Categorie);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }


            if (_dbContext.Products.Count() == 0)
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(DeliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
