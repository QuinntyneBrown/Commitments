// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using System.Linq;


namespace Commitments.Infrastructure.Data;

public class SeedData
{
    public static void Seed(CommitmentsDbContext context)
    {
        BehaviourTypeConfiguration.Seed(context);
        FrequencyTypeConfiguration.Seed(context);
    }

    internal class BehaviourTypeConfiguration
    {
        public static void Seed(CommitmentsDbContext context)
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
        public static void Seed(CommitmentsDbContext context)
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
}

