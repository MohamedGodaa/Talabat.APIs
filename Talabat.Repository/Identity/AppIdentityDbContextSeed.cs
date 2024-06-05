using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManger)
        {
            if (!userManger.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Mohamed Goda",
                    Email = "mohamed.goda1852@gmail.com",
                    UserName = "mohamed.goda1852",
                    PhoneNumber = "01117043794",
                };
                await userManger.CreateAsync(User, "Pa$$0rd");
            }
        }
    }
}
