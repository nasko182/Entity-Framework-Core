using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        [Key]
        public int UserId { get; set; }

        [MaxLength(256)]
        public string Username { get; set; } = null!;

        [MaxLength(256)]
        public string Password { get; set; } = null!;

        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
