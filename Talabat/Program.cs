using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repository.Contract;
using Talabat.Errors;
using Talabat.Exceptions;
using Talabat.Helpers;
using Talabat.Middlewares;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repository;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DeafultConnection"));
            });
            

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            
            builder.Services.AddAplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();


            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            var logger = LoggerFactory.CreateLogger<Program>();
            try
            {
                var _dbContext = Services.GetRequiredService<StoreContext>();
                await _dbContext.Database.MigrateAsync();

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(_dbContext);
                var userManger = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManger);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message}");

            }


            app.UseMiddleware<ExceptionMiddleware>();

            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerServices();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            //app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
