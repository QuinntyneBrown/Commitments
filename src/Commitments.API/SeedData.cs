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
            BehaviourTypeConfiguration.Seed(context);
            FrequencyTypeConfiguration.Seed(context);
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }


        internal class BehaviourTypeConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Acts Of Service") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Acts Of Service" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Physical Touch") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Physical Touch" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Gifts") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Gifts" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Words of Affirmation") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Words of Affirmation" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Quality Time") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Quality Time" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Respect") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Respect" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Listening") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Listening" });

                if (context.BehaviourTypes.FirstOrDefault(x => x.Name == "Health") == null)
                    context.BehaviourTypes.Add(new BehaviourType() { Name = "Health" });

                context.SaveChanges();
            }
        }

        internal class FrequencyTypeConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.FrequencyTypes.FirstOrDefault(x => x.Name == "per day") == null)
                    context.FrequencyTypes.Add(new FrequencyType() { Name = "per day" });

                if (context.FrequencyTypes.FirstOrDefault(x => x.Name == "per week") == null)
                    context.FrequencyTypes.Add(new FrequencyType() { Name = "per week" });

                if (context.FrequencyTypes.FirstOrDefault(x => x.Name == "per month") == null)
                    context.FrequencyTypes.Add(new FrequencyType() { Name = "per month" });

                context.SaveChanges();
            }
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
