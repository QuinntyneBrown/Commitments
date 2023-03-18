// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.AggregateModel.BehaviourTypeAggregate.Commands;

public class SaveBehaviourTypeCommandValidator : AbstractValidator<SaveBehaviourTypeRequest>
{
    public SaveBehaviourTypeCommandValidator()
    {
        RuleFor(request => request.BehaviourType.BehaviourTypeId).NotNull();
    }
}

public class SaveBehaviourTypeRequest : IRequest<SaveBehaviourTypeResponse>
{
    public BehaviourTypeDto BehaviourType { get; set; }
}

public class SaveBehaviourTypeResponse
{
    public Guid BehaviourTypeId { get; set; }
}

public class SaveBehaviourTypeCommandHandler : IRequestHandler<SaveBehaviourTypeRequest, SaveBehaviourTypeResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public SaveBehaviourTypeCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<SaveBehaviourTypeResponse> Handle(SaveBehaviourTypeRequest request, CancellationToken cancellationToken)
    {
        var behaviourType = await _context.BehaviourTypes.FindAsync(request.BehaviourType.BehaviourTypeId);

        if (behaviourType == null) _context.BehaviourTypes.Add(behaviourType = new BehaviourType());

        behaviourType.Name = request.BehaviourType.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return new SaveBehaviourTypeResponse() { BehaviourTypeId = behaviourType.BehaviourTypeId };
    }
}

