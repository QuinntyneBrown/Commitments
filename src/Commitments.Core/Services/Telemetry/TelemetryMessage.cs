// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MessagePack;

namespace Commitments.Core.Services.Telemetry;

[MessagePackObject]
public class TelemetryMessage
{
    public TelemetryMessage()
    {
        Created = DateTime.UtcNow;
        Service = AppDomain.CurrentDomain.FriendlyName.Split('.').First();
    }

    [Key(0)]
    public DateTime Created { get; set; }

    [Key(1)]
    public string Service { get; set; }
}
