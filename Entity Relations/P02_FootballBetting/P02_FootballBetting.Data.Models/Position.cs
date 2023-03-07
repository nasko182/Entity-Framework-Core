using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }
        [Key]
        public int PositionId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; }
    }
}
