// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.Commitments;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;
using Commitments.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace IntegrationTests.Features;

public class CommitmentScenarios: CommitmentScenarioBase
{

    [Fact]
    public async Task ShouldSave()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .PostAsAsync<SaveCommitmentCommand.Request, SaveCommitmentCommand.Response>(Post.Commitments, new SaveCommitmentCommand.Request() {
                    Commitment = new CommitmentApiModel()
                    {

                    }
                });

            Assert.True(response.CommitmentId != default(int));
        }
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetCommitmentsQuery.Response>(Get.Commitments);

            Assert.True(response.Commitments.Count() > 0);
        }
    }

    [Fact]
    public async Task ShouldGetDaily()
    {
        using (var server = CreateServer())
        {

            IAppDbContext context = server.Host.Services.GetService(typeof(IAppDbContext)) as IAppDbContext;
            Behaviour runningBehaviour = default(Behaviour);
            Behaviour washingDishesBehaviour = default(Behaviour);
            Frequency oncePerDay = default(Frequency);
            Frequency sevenTimesPerWeek = default(Frequency);

            Commitment runningCommitment = default(Commitment);

            Commitment washingDishesCommitment = default(Commitment);

            context.Behaviours.Add(runningBehaviour = new Behaviour()
            {
                Name = "Running",
                BehaviourTypeId = context.BehaviourTypes.Single(x => x.Name == "Health").BehaviourTypeId
            });

            context.Behaviours.Add(washingDishesBehaviour = new Behaviour()
            {
                Name = "Washing Dishes",
                BehaviourTypeId = context.BehaviourTypes.Single(x => x.Name == "Acts Of Service").BehaviourTypeId
            });

            var perDayFrequencyType = context.FrequencyTypes.Single(x => x.Name == "per day");

            var perWeekFrequencyType = context.FrequencyTypes.Single(x => x.Name == "per week");

            context.Frequencies.Add(oncePerDay = new Frequency()
            {
                FrequencyType = perDayFrequencyType,
                IsDesirable = true,
                Frequency = 1
            });

            context.Frequencies.Add(sevenTimesPerWeek = new Frequency()
            {
                FrequencyType = perWeekFrequencyType,
                IsDesirable = true,
                Frequency = 7
            });

            context.Commitments.Add(runningCommitment =  new Commitment()
            {
                Behaviour = runningBehaviour,
                ProfileId = 1,
                CommitmentFrequencies = new List<CommitmentFrequency>() { new CommitmentFrequency() { Frequency = oncePerDay, Commitment = runningCommitment } }
            });

            context.Commitments.Add(washingDishesCommitment = new Commitment()
            {
                Behaviour = washingDishesBehaviour,
                ProfileId = 1,
                CommitmentFrequencies = new List<CommitmentFrequency>() { new CommitmentFrequency() { Frequency = sevenTimesPerWeek, Commitment = washingDishesCommitment } }
            });

            await context.SaveChangesAsync(default(CancellationToken));

            var response = await server.CreateClient()
                .GetAsync<GetCommitmentsQuery.Response>(Get.DailyCommitments);

            Assert.Single(response.Commitments);
        }
    }


    [Fact]
    public async Task ShouldGetById()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .GetAsync<GetCommitmentByIdQuery.Response>(Get.CommitmentById(1));

            Assert.True(response.Commitment.CommitmentId != default(int));
        }
    }

    [Fact]
    public async Task ShouldUpdate()
    {
        using (var server = CreateServer())
        {
            var getByIdResponse = await server.CreateClient()
                .GetAsync<GetCommitmentByIdQuery.Response>(Get.CommitmentById(1));

            Assert.True(getByIdResponse.Commitment.CommitmentId != default(int));

            var saveResponse = await server.CreateClient()
                .PostAsAsync<SaveCommitmentCommand.Request, SaveCommitmentCommand.Response>(Post.Commitments, new SaveCommitmentCommand.Request()
                {
                    Commitment = getByIdResponse.Commitment
                });

            Assert.True(saveResponse.CommitmentId != default(int));
        }
    }

    [Fact]
    public async Task ShouldDelete()
    {
        using (var server = CreateServer())
        {
            var response = await server.CreateClient()
                .DeleteAsync(Delete.Commitment(1));

            response.EnsureSuccessStatusCode();
        }
    }
}

