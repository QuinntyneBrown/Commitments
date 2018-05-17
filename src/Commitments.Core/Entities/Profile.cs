using System.Collections.Generic;

namespace Commitments.Core.Entities
{
    public class Profile: User
    {        
        public int ProfileId { get; set; }           
		public string Name { get; set; }
        public ICollection<Commitment> Commitments { get; set; } = new HashSet<Commitment>();
    }
}
