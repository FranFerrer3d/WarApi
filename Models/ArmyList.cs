using WarApi.Models.Interfaces;

namespace WarApi.Models
{
    public class ArmyList : IArmyList
    {
        public Guid Id { get; set; }
        public string Faction { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
