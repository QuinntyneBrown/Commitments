// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate.Commands;

public class UpdateCardLayoutRequestValidator : AbstractValidator<UpdateCardLayoutRequest> { }

public class UpdateCardLayoutRequest : IRequest<UpdateCardLayoutResponse>
{
    public Guid CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


public class UpdateCardLayoutResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}


public class UpdateCardLayoutRequestHandler : IRequestHandler<UpdateCardLayoutRequest, UpdateCardLayoutResponse>
{
    private readonly ILogger<UpdateCardLayoutRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public UpdateCardLayoutRequestHandler(ILogger<UpdateCardLayoutRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateCardLayoutResponse> Handle(UpdateCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = await _context.CardLayouts.SingleAsync(x => x.CardLayoutId == request.CardLayoutId);

        cardLayout.CardLayoutId = request.CardLayoutId;
        cardLayout.Name = request.Name;
        cardLayout.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            CardLayout = cardLayout.ToDto()
        };

    }

}