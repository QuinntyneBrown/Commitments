using Commitments.Core.Entities;
using Commitments.Core.Identity;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Commitments.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                User user = default(User);

                if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
                {
                    user = new User()
                    {
                        Username = "quinntynebrown@gmail.com"
                    };

                    user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                    context.Users.Add(user);

                }
                
                context.Profiles.Add(new Profile()
                {
                    Name = "Quinntyne",
                    User = user
                });
                
                context.SaveChanges();
            }
        }

        internal class TagConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                context.SaveChanges();
            }
        }
    }
}
