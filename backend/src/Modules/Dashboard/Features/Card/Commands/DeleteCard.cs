using Commitments.Shared;
using Dashboard.Data;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Features.Card;

public class DeleteCardRequest : IRequest<DeleteCardResponse>
{
    public Guid CardId { get; set; }
}

public class DeleteCardResponse : ResponseBase { }

public class DeleteCardRequestHandler : IRequestHandler<DeleteCardRequest, DeleteCardResponse>
{
    private readonly IDashboardDbContext _context;

    public DeleteCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<DeleteCardResponse> Handle(DeleteCardRequest request, CancellationToken cancellationToken)
    {
        var referenceCount = _context.DashboardCards.Count(dc => dc.CardId == request.CardId);
        if (referenceCount > 0)
        {
            throw new BadHttpRequestException($"Cannot delete: referenced by {referenceCount} dashboard tile(s)", 400);
        }

        var card = await _context.Cards.FindAsync(request.CardId);
        if (card != null)
        {
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteCardResponse();
    }
}
