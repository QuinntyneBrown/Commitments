// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Shared;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;


namespace Commitments.Features.Activity;

public class RemoveActivityCommandValidator : AbstractValidator<RemoveActivityRequest>
{
    public RemoveActivityCommandValidator()
    {
        RuleFor(request => request.ActivityId).NotEqual(default(Guid));
    }
}

public class RemoveActivityRequest : IRequest
{
    public Guid ActivityId { get; set; }
}

public class RemoveActivityCommandHandler : IRequestHandler<RemoveActivityRequest>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public RemoveActivityCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task Handle(RemoveActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _context.Activities.FindAsync(request.ActivityId);
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new ActivityRecordedEvent
        {
            ActivityId = activity.ActivityId,
            BehaviourId = activity.BehaviourId,
            ProfileId = activity.ProfileId,
            PerformedOn = activity.PerformedOn,
            Reason = ActivityChangeReason.Deleted
        }, cancellationToken);
    }
}
