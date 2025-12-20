// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;

namespace Commitments.Core.AggregateModel.DigitalAssetAggregate.Commands;

public class CreateDigitalAssetRequestValidator : AbstractValidator<CreateDigitalAssetRequest>
{
    public CreateDigitalAssetRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateDigitalAssetRequest : IRequest<CreateDigitalAssetResponse>
{
    public string Name { get; set; } = null!;
    public string? ContentType { get; set; }
    public byte[]? Bytes { get; set; }
}

public class CreateDigitalAssetResponse
{
    public Guid DigitalAssetId { get; set; }
}

public class CreateDigitalAssetRequestHandler : IRequestHandler<CreateDigitalAssetRequest, CreateDigitalAssetResponse>
{
    private readonly ICommitmentsDbContext _context;

    public CreateDigitalAssetRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateDigitalAssetResponse> Handle(CreateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = new DigitalAsset
        {
            DigitalAssetId = Guid.NewGuid(),
            Name = request.Name,
            ContentType = request.ContentType,
            Bytes = request.Bytes
        };

        _context.DigitalAssets.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateDigitalAssetResponse
        {
            DigitalAssetId = entity.DigitalAssetId
        };
    }
}
