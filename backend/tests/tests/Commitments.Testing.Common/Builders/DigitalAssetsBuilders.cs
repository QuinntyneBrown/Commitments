using DigitalAssets.Core.Model.DigitalAssetAggregate;

namespace Commitments.Testing.Common.Builders;

public class DigitalAssetBuilder
{
    private Guid _digitalAssetId = Guid.NewGuid();
    private byte[]? _bytes = new byte[] { 1, 2, 3 };
    private string? _contentType = "image/png";
    private string? _name = "test.png";

    public DigitalAssetBuilder WithDigitalAssetId(Guid id) { _digitalAssetId = id; return this; }
    public DigitalAssetBuilder WithBytes(byte[]? bytes) { _bytes = bytes; return this; }
    public DigitalAssetBuilder WithContentType(string? ct) { _contentType = ct; return this; }
    public DigitalAssetBuilder WithName(string? name) { _name = name; return this; }

    public DigitalAsset Build() => new DigitalAsset
    {
        DigitalAssetId = _digitalAssetId,
        Bytes = _bytes,
        ContentType = _contentType,
        Name = _name
    };
}
