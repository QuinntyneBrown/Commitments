// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Model.CardLayoutAggregate;

public class CardLayout
{
    public CardLayout(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public Guid CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}