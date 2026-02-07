using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Card;

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
        var card = await _context.Cards.FindAsync(request.CardId);
        if (card != null)
        {
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteCardResponse();
    }
}
