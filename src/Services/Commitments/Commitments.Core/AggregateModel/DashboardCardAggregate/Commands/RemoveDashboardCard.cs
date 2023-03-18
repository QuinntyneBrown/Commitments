// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.DashboardCardAggregate.Commands;

public class RemoveDashboardCardCommandValidator : AbstractValidator<RemoveDashboardCardRequest>
{
    public RemoveDashboardCardCommandValidator()
    {
        RuleFor(request => request.DashboardCardId).NotEqual(default(Guid));
    }
}

public class RemoveDashboardCardRequest : IRequest
{
    public Guid DashboardCardId { get; set; }
}

public class RemoveDashboardCardCommandHandler : IRequestHandler<RemoveDashboardCardRequest>
{
    public ICommimentsDbContext _context { get; set; }

    public RemoveDashboardCardCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task Handle(RemoveDashboardCardRequest request, CancellationToken cancellationToken)
    {
        _context.DashboardCards.Remove(await _context.DashboardCards.FindAsync(request.DashboardCardId));
        await _context.SaveChangesAsync(cancellationToken);
    }

}

