using System;

namespace Commitments.Core.Entities
{
    public class DigitalAsset
    {
        public Guid DigitalAssetId { get; set; }           
        public string Name { get; set; }        
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
    }
}
