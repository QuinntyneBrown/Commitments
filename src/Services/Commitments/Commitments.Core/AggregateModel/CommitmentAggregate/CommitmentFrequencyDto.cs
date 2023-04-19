// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.FrequencyAggregate;

namespace Commitments.Core.AggregateModel.CommitmentAggregate;

public class CommitmentFrequencyDto
{
    public Guid CommitmentFrequencyId { get; set; }
    public string Name { get; set; }
    public Guid? FrequencyId { get; set; }
    public FrequencyDto Frequency { get; set; }
    public static CommitmentFrequencyDto FromCommitmentFrequency(CommitmentFrequency commitmentFrequency)
    {
        var model = new CommitmentFrequencyDto();
        model.CommitmentFrequencyId = commitmentFrequency.CommitmentFrequencyId;
        model.FrequencyId = commitmentFrequency.FrequencyId;
        model.Frequency = FrequencyDto.FromFrequency(commitmentFrequency.Frequency);
        return model;
    }
}