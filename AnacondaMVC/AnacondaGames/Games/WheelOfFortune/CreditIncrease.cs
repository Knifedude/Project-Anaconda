using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnacondaMVC.Games.WheelOfFortune
{
    public class CreditIncrease : ISpinAction
    {
        private int _creditsBonus;

        public CreditIncrease(int creditsBonus)
        {
            _creditsBonus = creditsBonus;
        }

        public GameResult Execute(GameContext context)
        {
            return new GameResult() { CreditsGained = _creditsBonus, Bet = context.Bet };
        }
    }
}