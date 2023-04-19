// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;


namespace Commitments.Core.AggregateModel.FrequencyTypeAggregate.Queries;

public class GetFrequencyTypeByIdValidator : AbstractValidator<GetFrequencyTypeByIdRequest>
{
    public GetFrequencyTypeByIdValidator()
    {
        RuleFor(request => request.FrequencyTypeId).NotEqual(default(Guid));
    }
}

public class GetFrequencyTypeByIdRequest : IRequest<GetFrequencyTypeByIdResponse>
{
    public Guid FrequencyTypeId { get; set; }
}

public class GetFrequencyTypeByIdResponse
{
    public FrequencyTypeDto FrequencyType { get; set; }
}

public class GetFrequencyTypeByIdHandler : IRequestHandler<GetFrequencyTypeByIdRequest, GetFrequencyTypeByIdResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetFrequencyTypeByIdHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetFrequencyTypeByIdResponse> Handle(GetFrequencyTypeByIdRequest request, CancellationToken cancellationToken)
        => new GetFrequencyTypeByIdResponse()
        {
            FrequencyType = FrequencyTypeDto.FromFrequencyType(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId))
        };
}