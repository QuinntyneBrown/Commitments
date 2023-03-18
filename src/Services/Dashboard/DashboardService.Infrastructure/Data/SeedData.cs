namespace DashboardService.Infrastructure.Data;

public static class SeedData
{
    public static void Seed(this DashboardServiceDbContext context)
    {
        CardLayoutConfiguration.Seed(context);
        CardConfiguration.Seed(context);
    }

    internal class CardLayoutConfiguration
    {
        public static void Seed(DashboardServiceDbContext context)
        {
            if (context.CardLayouts.FirstOrDefault(x => x.Name == "Poster") == null)
                context.CardLayouts.Add(new CardLayout("Poster", ""));

            context.SaveChanges();
        }
    }

    internal class CardConfiguration
    {
        public static void Seed(DashboardServiceDbContext context)
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
}
