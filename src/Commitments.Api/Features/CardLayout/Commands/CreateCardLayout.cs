// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Model.CardLayoutAggregate;
using Commitments.Core.Services.Kernel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using CardLayoutEntity = Commitments.Core.Model.CardLayoutAggregate.CardLayout;

namespace Commitments.Api.Features.CardLayout;

public class CreateCardLayoutRequestValidator : AbstractValidator<CreateCardLayoutRequest> { }

public class CreateCardLayoutRequest : IRequest<CreateCardLayoutResponse>
{
    public Guid CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


public class CreateCardLayoutResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}


public class CreateCardLayoutRequestHandler : IRequestHandler<CreateCardLayoutRequest, CreateCardLayoutResponse>
{
    private readonly ILogger<CreateCardLayoutRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public CreateCardLayoutRequestHandler(ILogger<CreateCardLayoutRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateCardLayoutResponse> Handle(CreateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = new CardLayoutEntity(request.Name, request.Description);

        _context.CardLayouts.Add(cardLayout);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            CardLayout = cardLayout.ToDto()
        };
    }
}