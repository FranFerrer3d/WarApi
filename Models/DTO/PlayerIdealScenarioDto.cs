using System.Collections.Generic;

namespace WarApi.Dtos
{
    public class PlayerIdealScenarioDto
    {
        public List<string> OpponentFactions { get; set; } = new();
        public List<string> Maps { get; set; } = new();
        public List<string> PrimaryMissions { get; set; } = new();
        public List<string> SecondaryMissions { get; set; } = new();
    }
}
