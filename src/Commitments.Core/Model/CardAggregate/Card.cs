// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Model.CardAggregate;

public class Card
{
    public Guid CardId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}