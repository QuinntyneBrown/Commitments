// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardAggregate.Commands;

public class UpdateCardRequestValidator : AbstractValidator<UpdateCardRequest> { }

public class UpdateCardRequest : IRequest<UpdateCardResponse>
{
    public Guid CardId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


public class UpdateCardResponse : ResponseBase
{
    public CardDto Card { get; set; }
}


public class UpdateCardRequestHandler : IRequestHandler<UpdateCardRequest, UpdateCardResponse>
{
    private readonly ILogger<UpdateCardRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public UpdateCardRequestHandler(ILogger<UpdateCardRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateCardResponse> Handle(UpdateCardRequest request, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.SingleAsync(x => x.CardId == request.CardId);

        card.CardId = request.CardId;
        card.Name = request.Name;
        card.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Card = card.ToDto()
        };

    }

}