// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.AggregateModel.ActivityAggregate.Commands;

public class SaveActivityCommandValidator : AbstractValidator<SaveActivityRequest>
{
    public SaveActivityCommandValidator()
    {
        RuleFor(request => request.Activity.ActivityId).NotNull();
    }
}

public class SaveActivityRequest : IRequest<SaveActivityResponse>
{
    public ActivityDto Activity { get; set; }
}

public class SaveActivityResponse
{
    public Guid ActivityId { get; set; }
}

public class SaveActivityCommandHandler : IRequestHandler<SaveActivityRequest, SaveActivityResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public SaveActivityCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task<SaveActivityResponse> Handle(SaveActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _context.Activities.FindAsync(request.Activity.ActivityId);

        if (activity == null) _context.Activities.Add(activity = new Activity());

        activity.BehaviourId = request.Activity.BehaviourId;
        activity.ProfileId = request.Activity.ProfileId;
        activity.PerformedOn = request.Activity.PerformedOn;
        activity.Description = request.Activity.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return new SaveActivityResponse() { ActivityId = activity.ActivityId };
    }
}

