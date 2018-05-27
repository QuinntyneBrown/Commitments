using Commitments.Core.Entities;

namespace Commitments.API.Features.DigitalAssets
{
    public class DigitalAssetApiModel
    {        
        public int DigitalAssetId { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public static DigitalAssetApiModel FromDigitalAsset(DigitalAsset digitalAsset)
            => new DigitalAssetApiModel
            {
                DigitalAssetId = digitalAsset.DigitalAssetId,
                Name = digitalAsset.Name,
                RelativePath = digitalAsset.RelativePath
            };
    }
}
