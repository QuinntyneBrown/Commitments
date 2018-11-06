using Commitments.API.Features.DashboardCards;
using Commitments.Core.Entities;
using Commitments.Core.Identity;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Commitments.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            //Console.WriteLine("WTF");
            //BehaviourTypeConfiguration.Seed(context);
            CardConfiguration.Seed(context);
            //CardLayoutConfiguration.Seed(context);

            FrequencyTypeConfiguration.Seed(context);
            //UserConfiguration.Seed(context);
            //TagConfiguration.Seed(context);

            //DashboardConfiguration.Seed(context);
            //DashboardCardConfiguration.Seed(context);
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

        internal class CardLayoutConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.CardLayouts.FirstOrDefault(x => x.Name == "Poster") == null)
                    context.CardLayouts.Add(new CardLayout() { Name = "Poster" });

                context.SaveChanges();
            }
        }

        internal class CardConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Cards.FirstOrDefault(x => x.Name == "Daily Results") == null)
                    context.Cards.Add(new Card() { Name = "Daily Results" });

                if (context.Cards.FirstOrDefault(x => x.Name == "Weekly Results") == null)
                    context.Cards.Add(new Card() { Name = "Weekly Results" });

                if (context.Cards.FirstOrDefault(x => x.Name == "Monthly Results") == null)
                    context.Cards.Add(new Card() { Name = "Monthly Results" });

                if (context.Cards.FirstOrDefault(x => x.Name == "Relations") == null)
                    context.Cards.Add(new Card() { Name = "Relations" });

                if (context.Cards.FirstOrDefault(x => x.Name == "Outstanding To Do's") == null)
                    context.Cards.Add(new Card() { Name = "Outstanding To Do's" });

                context.SaveChanges();
            }
        }

        internal class DashboardConfiguration {

            public static void Seed(AppDbContext context)
            {
                if(context.Dashboards.FirstOrDefault(x=> x.Name == "Default" && x.ProfileId == 1) == null)
                {
                    context.Dashboards.Add(new Dashboard()
                    {
                        Name = "Default",
                        ProfileId = 1
                    });
                }

                context.SaveChanges();
            }
        }

        internal class DashboardCardConfiguration
        {

            public static void Seed(AppDbContext context)
            {
                var dashboard = context.Dashboards.Include(x => x.DashboardCards).First(x => x.ProfileId == 1);

                if (dashboard.DashboardCards.SingleOrDefault(x => x.CardId == 1) == null)
                {
                    dashboard.DashboardCards.Add(new DashboardCard()
                    {
                        CardId = 1,
                        Options = JsonConvert.SerializeObject(new DashboardCardApiModel.OptionsApiModel()
                        {
                            Top = 1,
                            Left = 1,
                            Width = 1,
                            Height = 1

                        })
                    });
                }
                
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

                if (context.FrequencyTypes.FirstOrDefault(x => x.Name == "per 48 hours / per 72 after 3 occurrences") == null)
                    context.FrequencyTypes.Add(new FrequencyType() { Name = "per 48 hours / per 72 after 3 occurrences" });

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
