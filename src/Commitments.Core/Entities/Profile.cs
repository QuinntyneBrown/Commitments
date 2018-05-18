using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Entities
{
    public class Profile: BaseEntity
    {        
        public int ProfileId { get; set; }           
		public string Name { get; set; }
        public ICollection<Behaviour> Commitments { get; set; } = new HashSet<Behaviour>();        
        public User User { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
