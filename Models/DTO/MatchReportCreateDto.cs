using System.ComponentModel.DataAnnotations;

namespace WarApi.Dtos
{
    public class MatchReportCreateDto
    {
        [Required]
        public Guid PlayerAId { get; set; }

        [Required]
        public Guid PlayerBId { get; set; }

        public string ListA { get; set; } = string.Empty;
        public string ListB { get; set; } = string.Empty;

        public int ExpectedA { get; set; }
        public int ExpectedB { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string Map { get; set; } = string.Empty;
        public string Deployment { get; set; } = string.Empty;
        public string PrimaryMission { get; set; } = string.Empty;
        public string SecondaryA { get; set; } = string.Empty;
        public string SecondaryB { get; set; } = string.Empty;

        public int[] MagicA { get; set; } = new int[6];
        public int[] MagicB { get; set; } = new int[6];

        public int KillsA { get; set; }
        public int KillsB { get; set; }

        public string PrimaryResult { get; set; } = "None"; // Puede ser PlayerA, PlayerB, Both, None
        public bool SecondaryWinA { get; set; }
        public bool SecondaryWinB { get; set; }
    }
}
