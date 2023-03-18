// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.BehaviourAggregate.Queries;

public class GetBehaviourByIdValidator : AbstractValidator<GetBehaviourByIdRequest>
{
    public GetBehaviourByIdValidator()
    {
        RuleFor(request => request.BehaviourId).NotEqual(default(Guid));
    }
}

public class GetBehaviourByIdRequest : IRequest<GetBehaviourByIdResponse>
{
    public Guid BehaviourId { get; set; }
}

public class GetBehaviourByIdResponse
{
    public BehaviourDto Behaviour { get; set; }
}

public class GetBehaviourByIdHandler : IRequestHandler<GetBehaviourByIdRequest, GetBehaviourByIdResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetBehaviourByIdHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetBehaviourByIdResponse> Handle(GetBehaviourByIdRequest request, CancellationToken cancellationToken)
        => new GetBehaviourByIdResponse()
        {
            Behaviour = BehaviourDto.FromBehaviour(await _context.Behaviours
                .Include(x => x.BehaviourType)
                .SingleAsync(x => x.BehaviourId == request.BehaviourId))
        };
}

