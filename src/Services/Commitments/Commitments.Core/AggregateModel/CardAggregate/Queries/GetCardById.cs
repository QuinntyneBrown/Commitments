// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.CardAggregate.Queries;

public class GetCardByIdValidator : AbstractValidator<GetCardByIdRequest>
{
    public GetCardByIdValidator()
    {
        RuleFor(request => request.CardId).NotEqual(default(Guid));
    }
}

public class GetCardByIdRequest : IRequest<GetCardByIdResponse>
{
    public Guid CardId { get; set; }
}

public class GetCardByIdResponse
{
    public CardDto Card { get; set; }
}

public class GetCardByIdHandler : IRequestHandler<GetCardByIdRequest, GetCardByIdResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetCardByIdHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetCardByIdResponse> Handle(GetCardByIdRequest request, CancellationToken cancellationToken)
        => new GetCardByIdResponse()
        {
            Card = CardDto.FromCard(await _context.Cards.FindAsync(request.CardId))
        };
}

