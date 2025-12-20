// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Settings;

public class CorsSettings
{
    public const string Section = "Cors";

    public string[] Origins { get; set; } = Array.Empty<string>();
}
