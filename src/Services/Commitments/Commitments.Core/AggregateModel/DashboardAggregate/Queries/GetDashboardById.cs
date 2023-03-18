// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.DashboardAggregate.Queries;

public class GetDashboardByIdValidator : AbstractValidator<GetDashboardByIdRequest>
{
    public GetDashboardByIdValidator()
    {
        RuleFor(request => request.DashboardId).NotEqual(default(Guid));
    }
}

public class GetDashboardByIdRequest : IRequest<GetDashboardByIdResponse>
{
    public Guid DashboardId { get; set; }
}

public class GetDashboardByIdResponse
{
    public DashboardDto Dashboard { get; set; }
}

public class GetDashboardByIdHandler : IRequestHandler<GetDashboardByIdRequest, GetDashboardByIdResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetDashboardByIdHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetDashboardByIdResponse> Handle(GetDashboardByIdRequest request, CancellationToken cancellationToken)
        => new GetDashboardByIdResponse()
        {
            Dashboard = DashboardDto.FromDashboard(await _context.Dashboards.FindAsync(request.DashboardId))
        };
}

