// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate.Commands;

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

    private readonly IDashboardServiceDbContext _context;

    public CreateCardLayoutRequestHandler(ILogger<CreateCardLayoutRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateCardLayoutResponse> Handle(CreateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = new CardLayout(request.Name, request.Description);

        _context.CardLayouts.Add(cardLayout);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            CardLayout = cardLayout.ToDto()
        };
    }
}