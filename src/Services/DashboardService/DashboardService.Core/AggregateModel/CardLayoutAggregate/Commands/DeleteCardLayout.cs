// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate.Commands;

public class DeleteCardLayoutRequestValidator : AbstractValidator<DeleteCardLayoutRequest> { }

public class DeleteCardLayoutRequest : IRequest<DeleteCardLayoutResponse>
{
    public Guid CardLayoutId { get; set; }
}


public class DeleteCardLayoutResponse : ResponseBase
{
    public CardLayoutDto CardLayout { get; set; }
}


public class DeleteCardLayoutRequestHandler : IRequestHandler<DeleteCardLayoutRequest, DeleteCardLayoutResponse>
{
    private readonly ILogger<DeleteCardLayoutRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public DeleteCardLayoutRequestHandler(ILogger<DeleteCardLayoutRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteCardLayoutResponse> Handle(DeleteCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayoutId);

        _context.CardLayouts.Remove(cardLayout);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            CardLayout = cardLayout.ToDto()
        };
    }

}



