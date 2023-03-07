using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.Towns= new HashSet<Town>();
        }
        [Key]
        public int CountryId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Town> Towns { get; set; }
    }
}
