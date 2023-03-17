// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Core.AggregateModel;

public class CardLayout: BaseEntity
{
    public string Name { get; set; }
    public int CardLayoutId { get; set; }           
    public string Description { get; set; }
}

