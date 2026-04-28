// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Shared;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ActivityEntity = Commitments.Domain.ActivityAggregate.Activity;


namespace Commitments.Features.Activity;

public class SaveActivityCommandValidator : AbstractValidator<SaveActivityRequest>
{
    public SaveActivityCommandValidator()
    {
        RuleFor(request => request.Activity.ActivityId).NotEmpty();
        RuleFor(request => request.Activity.BehaviourId).NotEmpty();
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
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public SaveActivityCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<SaveActivityResponse> Handle(SaveActivityRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.Activities.FindAsync(request.Activity.ActivityId);
        var reason = existing is null ? ActivityChangeReason.Created : ActivityChangeReason.Updated;

        if (existing == null) _context.Activities.Add(existing = new ActivityEntity());

        existing.BehaviourId = request.Activity.BehaviourId;
        existing.ProfileId = request.Activity.ProfileId;
        existing.PerformedOn = request.Activity.PerformedOn;
        existing.Description = request.Activity.Description;

        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new ActivityRecordedEvent
        {
            ActivityId = existing.ActivityId,
            BehaviourId = existing.BehaviourId,
            ProfileId = existing.ProfileId,
            PerformedOn = existing.PerformedOn,
            Reason = reason
        }, cancellationToken);

        return new SaveActivityResponse() { ActivityId = existing.ActivityId };
    }
}
