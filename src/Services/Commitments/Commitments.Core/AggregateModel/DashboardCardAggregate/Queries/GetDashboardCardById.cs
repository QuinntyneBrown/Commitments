// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.DashboardCardAggregate.Queries;

public class GetDashboardCardByIdValidator : AbstractValidator<GetDashboardCardByIdRequest>
{
    public GetDashboardCardByIdValidator()
    {
        RuleFor(request => request.DashboardCardId).NotEqual(default(Guid));
    }
}

public class GetDashboardCardByIdRequest : IRequest<GetDashboardCardByIdResponse>
{
    public Guid DashboardCardId { get; set; }
}

public class GetDashboardCardByIdResponse
{
    public DashboardCardDto DashboardCard { get; set; }
}

public class GetDashboardCardByIdHandler : IRequestHandler<GetDashboardCardByIdRequest, GetDashboardCardByIdResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetDashboardCardByIdHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetDashboardCardByIdResponse> Handle(GetDashboardCardByIdRequest request, CancellationToken cancellationToken)
        => new GetDashboardCardByIdResponse()
        {
            DashboardCard = DashboardCardDto.FromDashboardCard(await _context.DashboardCards.FindAsync(request.DashboardCardId))
        };
}

