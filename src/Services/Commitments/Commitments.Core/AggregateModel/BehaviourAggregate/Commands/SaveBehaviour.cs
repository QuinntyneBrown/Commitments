// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Extensions;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.AggregateModel.BehaviourAggregate.Commands;

public class SaveBehaviourCommandValidator : AbstractValidator<SaveBehaviourRequest>
{
    public SaveBehaviourCommandValidator()
    {
        RuleFor(request => request.Behaviour.BehaviourId).NotNull();
    }
}

public class SaveBehaviourRequest : IRequest<SaveBehaviourResponse>
{
    public BehaviourDto Behaviour { get; set; }
}

public class SaveBehaviourResponse
{
    public Guid BehaviourId { get; set; }
}

public class SaveBehaviourCommandHandler : IRequestHandler<SaveBehaviourRequest, SaveBehaviourResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public SaveBehaviourCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<SaveBehaviourResponse> Handle(SaveBehaviourRequest request, CancellationToken cancellationToken)
    {
        var behaviour = await _context.Behaviours.FindAsync(request.Behaviour.BehaviourId);

        if (behaviour == null) _context.Behaviours.Add(behaviour = new Behaviour());

        behaviour.Name = request.Behaviour.Name;
        behaviour.Slug = request.Behaviour.Name.GenerateSlug();
        behaviour.Description = request.Behaviour.Description;
        behaviour.BehaviourTypeId = request.Behaviour.BehaviourTypeId;
        await _context.SaveChangesAsync(cancellationToken);

        return new SaveBehaviourResponse() { BehaviourId = behaviour.BehaviourId };
    }
}

