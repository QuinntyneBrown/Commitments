// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.DashboardAggregate.Commands;

public class RemoveDashboardCommandValidator : AbstractValidator<RemoveDashboardRequest>
{
    public RemoveDashboardCommandValidator()
    {
        RuleFor(request => request.DashboardId).NotEqual(default(Guid));
    }
}

public class RemoveDashboardRequest : IRequest
{
    public Guid DashboardId { get; set; }
}

public class RemoveDashboardCommandHandler : IRequestHandler<RemoveDashboardRequest>
{
    public ICommimentsDbContext _context { get; set; }

    public RemoveDashboardCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task Handle(RemoveDashboardRequest request, CancellationToken cancellationToken)
    {
        _context.Dashboards.Remove(await _context.Dashboards.FindAsync(request.DashboardId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

