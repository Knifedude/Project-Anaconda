using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnacondaMVC.Games
{
    public class GameResult
    {
        public int CreditsGained { get; set; }

        public int LuckIncreased { get; set; }

        public decimal CreditMultiplier { get; set; }

        public int ExperienceGained { get; set; }

        public decimal ExperienceMultiplier { get; set; }

        public int Bet { get; set; }
    }
}