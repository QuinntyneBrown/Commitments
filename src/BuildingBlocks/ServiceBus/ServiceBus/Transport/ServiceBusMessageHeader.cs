// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Transport;

public class ServiceBusMessageHeader {

    public static int SizeInBytes = 20;
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public string Topic { get; set; } = string.Empty;
    public int SequenceId { get; set; }
    public List<Rule> Rules { get; set; } = new List<Rule>();
}
