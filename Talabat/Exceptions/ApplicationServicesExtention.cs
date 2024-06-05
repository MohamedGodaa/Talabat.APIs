using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Errors;
using Talabat.Helpers;
using Talabat.Repository;
using Talabat.Repository.Repository;
using Talabat.Services;

namespace Talabat.Exceptions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<Product>, GenaricRepository<Product>>();
            services.AddScoped<IGenericRepository<ProductBrand>, GenaricRepository<ProductBrand>>();
            services.AddScoped<IGenericRepository<Productcategories>, GenaricRepository<Productcategories>>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService,PaymentService>();
            
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToList();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
            
        }
    }
}
