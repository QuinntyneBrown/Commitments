namespace Commitments.Core.Entities
{
    public class DigitalAsset
    {
        public int DigitalAssetId { get; set; }           
        public string Name { get; set; }        
        public byte[] Bytes { get; set; }
        public string RelativePath { get; set; }
    }
}
