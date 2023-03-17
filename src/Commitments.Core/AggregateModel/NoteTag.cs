// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Core.AggregateModel;

public class NoteTag: BaseEntity
{
    public int NoteTagId { get; set; }
    public int NoteId { get; set; }
    public int TagId { get; set; }
    public Note Note { get; set; }
    public Tag Tag { get; set; }
}        

