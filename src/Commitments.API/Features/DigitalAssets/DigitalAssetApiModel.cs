using Commitments.Core.Entities;

namespace Commitments.API.Features.DigitalAssets
{
    public class DigitalAssetApiModel
    {        
        public int DigitalAssetId { get; set; }
        public string Name { get; set; }

        public static DigitalAssetApiModel FromDigitalAsset(DigitalAsset digitalAsset)
        {
            var model = new DigitalAssetApiModel();
            model.DigitalAssetId = digitalAsset.DigitalAssetId;
            model.Name = digitalAsset.Name;
            return model;
        }
    }
}
