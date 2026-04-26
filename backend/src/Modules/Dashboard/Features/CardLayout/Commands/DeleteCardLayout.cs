using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.CardLayout;

public class DeleteCardLayoutRequest : IRequest<DeleteCardLayoutResponse>
{
    public Guid CardLayoutId { get; set; }
}

public class DeleteCardLayoutResponse : ResponseBase { }

public class DeleteCardLayoutRequestHandler : IRequestHandler<DeleteCardLayoutRequest, DeleteCardLayoutResponse>
{
    private readonly IDashboardDbContext _context;

    public DeleteCardLayoutRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<DeleteCardLayoutResponse> Handle(DeleteCardLayoutRequest request, CancellationToken cancellationToken)
    {
        var cardLayout = await _context.CardLayouts.FindAsync(request.CardLayoutId);
        if (cardLayout != null)
        {
            _context.CardLayouts.Remove(cardLayout);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteCardLayoutResponse();
    }
}
