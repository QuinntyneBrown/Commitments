// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.CardAggregate.Commands;

public class RemoveCardCommandValidator : AbstractValidator<RemoveCardRequest>
{
    public RemoveCardCommandValidator()
    {
        RuleFor(request => request.CardId).NotEqual(default(Guid));
    }
}

public class RemoveCardRequest : IRequest<ResponseBase>
{
    public Guid CardId { get; set; }
}

public class RemoveCardCommandHandler : IRequestHandler<RemoveCardRequest, ResponseBase>
{
    public ICommimentsDbContext _context { get; set; }

    public RemoveCardCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task<ResponseBase> Handle(RemoveCardRequest request, CancellationToken cancellationToken)
    {
        _context.Cards.Remove(await _context.Cards.FindAsync(request.CardId));

        await _context.SaveChangesAsync(cancellationToken);

        return new();
    }

}

