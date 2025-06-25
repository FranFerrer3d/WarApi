using System;
using System.Collections.Generic;
using WarApi.Models;

namespace MatchReportNamespace
{
    public enum PrimaryWinner
    {
        None,
        PlayerA,
        PlayerB,
        Both
    }

    public interface IMatchReport
    {
        Guid Id { get; set; }

        // Player info
        // Foreign Key + Navigation for PlayerA
        Guid PlayerAId { get; set; } 
        Player PlayerA { get; set; } 

        // Foreign Key + Navigation for PlayerB
        Guid PlayerBId { get; set; }
        Player PlayerB { get; set; }
        string ListA { get; set; }
        string ListB { get; set; }
        string ArmyA { get; set; }
        string ArmyB { get; set; }
        int ExpectedA { get; set; }
        int ExpectedB { get; set; }

        // Match setup
        DateTime Date { get; set; }
        string Map { get; set; }
        string Deployment { get; set; }
        string PrimaryMission { get; set; }
        string SecondaryA { get; set; }
        string SecondaryB { get; set; }

        // Magic phases
        int[] MagicA { get; set; } // 6 elements
        int[] MagicB { get; set; }

        // Points destroyed
        int KillsA { get; set; }
        int KillsB { get; set; }

        // Outcome
        PrimaryWinner PrimaryResult { get; set; }
        bool SecondaryWinA { get; set; }
        bool SecondaryWinB { get; set; }
        //Dictionary<int, int> FinalScore { get; set; } // e.g., {20, 0} or {13, 7}
        public int FinalScoreA { get; set; }
        public int FinalScoreB { get; set; }

    }
}
