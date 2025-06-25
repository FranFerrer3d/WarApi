using System;
using System.Collections.Generic;
using WarApi.Models;

namespace MatchReportNamespace
{
    public class MatchReport : IMatchReport
    {
        public Guid Id { get; set; }

        // Foreign Key + Navigation for PlayerA
        public Guid PlayerAId { get; set; } // EF usará esto en DB
        public Player PlayerA { get; set; } = null!; // Navegación en C#

        // Foreign Key + Navigation for PlayerB
        public Guid PlayerBId { get; set; }
        public Player PlayerB { get; set; } = null!;


        public string ListA { get; set; }
        public string ListB { get; set; }
        public string ArmyA { get; set; } = string.Empty;
        public string ArmyB { get; set; } = string.Empty;
        public int ExpectedA { get; set; }
        public int ExpectedB { get; set; }

        // Match setup
        public DateTime Date { get; set; }
        public string Map { get; set; }
        public string Deployment { get; set; }
        public string PrimaryMission { get; set; }
        public string SecondaryA { get; set; }
        public string SecondaryB { get; set; }

        // Magic phases
        public int[] MagicA { get; set; }
        public int[] MagicB { get; set; }

        // Points destroyed
        public int KillsA { get; set; }
        public int KillsB { get; set; }

        // Outcome
        public PrimaryWinner PrimaryResult { get; set; }
        public bool SecondaryWinA { get; set; }
        public bool SecondaryWinB { get; set; }
        //public Dictionary<int, int> FinalScore { get; set; }
        public int FinalScoreA { get; set; }
        public int FinalScoreB { get; set; }


        // Constructor
        public MatchReport()
        {
            ListA = "";
            ListB = "";
            ArmyA = "";
            ArmyB = "";
            MagicA = new int[6];
            MagicB = new int[6];
            Date = DateTime.Now;
        }

    }
}
