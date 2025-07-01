using System.ComponentModel.DataAnnotations;

namespace WarApi.Models
{
    public class Team
    {
        [Key]
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
