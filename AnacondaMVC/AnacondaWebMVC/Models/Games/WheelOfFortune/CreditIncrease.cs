using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnacondaMVC.Games.WheelOfFortune
{
    public class CreditIncrease : ISpinResult
    {
        private int _creditsBonus;

        public CreditIncrease(int creditsBonus)
        {
            _creditsBonus = creditsBonus;
        }

        public GameResult Execute(GameContext context)
        {
            //TODO: Increase player credits with _creditsBonus

            return new GameResult() { CreditsGained = _creditsBonus };
        }
    }
}