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
            MagicA = new int[6];
            MagicB = new int[6];
            Date = DateTime.Now;
        }

        public void CalculateFinalScore()
        {
            int pointDiff = KillsA - KillsB;
            double percentage = (double)Math.Abs(pointDiff) / 4000.0;

            // Scale to 0–16
            int basePoints = (int)Math.Round(percentage * 16);
            if (basePoints > 16) basePoints = 16;

            int pointsA = pointDiff > 0 ? basePoints : 16 - basePoints;
            int pointsB = 16 - pointsA;

            // Add mission bonuses
            if (PrimaryResult == PrimaryWinner.PlayerA) pointsA += 3;
            else if (PrimaryResult == PrimaryWinner.PlayerB) pointsB += 3;
            else if (PrimaryResult == PrimaryWinner.Both)
            {
                pointsA += 3;
                pointsB += 3;
            }

            if (SecondaryWinA) pointsA += 1;
            if (SecondaryWinB) pointsB += 1;

            // Clamp to 0–20
            pointsA = Math.Clamp(pointsA, 0, 20);
            pointsB = Math.Clamp(pointsB, 0, 20);

            // Ensure total is 20
            if (pointsA + pointsB > 20)
            {
                int excess = (pointsA + pointsB) - 20;
                if (pointsA > pointsB)
                    pointsA -= excess;
                else
                    pointsB -= excess;
            }
            this.FinalScoreA = pointsA;
            this.FinalScoreB = pointsB;

        }
    }
}
