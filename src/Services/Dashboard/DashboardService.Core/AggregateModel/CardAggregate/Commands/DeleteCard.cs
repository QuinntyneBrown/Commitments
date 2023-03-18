// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardAggregate.Commands;

public class DeleteCardRequestValidator : AbstractValidator<DeleteCardRequest> { }

public class DeleteCardRequest : IRequest<DeleteCardResponse>
{
    public Guid CardId { get; set; }
}


public class DeleteCardResponse : ResponseBase
{
    public CardDto Card { get; set; }
}


public class DeleteCardRequestHandler : IRequestHandler<DeleteCardRequest, DeleteCardResponse>
{
    private readonly ILogger<DeleteCardRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public DeleteCardRequestHandler(ILogger<DeleteCardRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteCardResponse> Handle(DeleteCardRequest request, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.FindAsync(request.CardId);

        _context.Cards.Remove(card);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Card = card.ToDto()
        };
    }

}



