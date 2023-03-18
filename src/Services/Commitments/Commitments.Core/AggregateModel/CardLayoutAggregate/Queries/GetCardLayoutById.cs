// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.CardLayoutAggregate.Queries;

public class GetCardLayoutByIdValidator : AbstractValidator<GetCardLayoutByIdRequest>
{
    public GetCardLayoutByIdValidator()
    {
        RuleFor(request => request.CardLayoutId).NotEqual(default(Guid));
    }
}

public class GetCardLayoutByIdRequest : IRequest<GetCardLayoutByIdResponse>
{
    public Guid CardLayoutId { get; set; }
}

public class GetCardLayoutByIdResponse
{
    public CardLayoutDto CardLayout { get; set; }
}

public class GetCardLayoutByIdHandler : IRequestHandler<GetCardLayoutByIdRequest, GetCardLayoutByIdResponse>
{
    public ICommimentsDbContext _context { get; set; }


    public async Task<GetCardLayoutByIdResponse> Handle(GetCardLayoutByIdRequest request, CancellationToken cancellationToken)
        => new GetCardLayoutByIdResponse()
        {
            CardLayout = CardLayoutDto.FromCardLayout(await _context.CardLayouts.FindAsync(request.CardLayoutId))
        };
}

