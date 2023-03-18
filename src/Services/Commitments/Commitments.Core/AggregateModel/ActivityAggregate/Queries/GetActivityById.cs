// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.ActivityAggregate.Queries;

public class GetActivityByIdValidator : AbstractValidator<GetActivityByIdRequest>
{
    public GetActivityByIdValidator()
    {
        RuleFor(request => request.ActivityId).NotEqual(default(Guid));
    }
}

public class GetActivityByIdRequest : IRequest<GetActivityByIdResponse>
{
    public Guid ActivityId { get; set; }
}

public class GetActivityByIdResponse
{
    public ActivityDto Activity { get; set; }
}

public class GetActivityByIdHandler : IRequestHandler<GetActivityByIdRequest, GetActivityByIdResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetActivityByIdHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetActivityByIdResponse> Handle(GetActivityByIdRequest request, CancellationToken cancellationToken)
        => new GetActivityByIdResponse()
        {
            Activity = ActivityDto.FromActivity(await _context.Activities
                .Include(x => x.Behaviour)
                .Include("Behaviour.BehaviourType")
                .SingleAsync(x => x.ActivityId == request.ActivityId))
        };
}

