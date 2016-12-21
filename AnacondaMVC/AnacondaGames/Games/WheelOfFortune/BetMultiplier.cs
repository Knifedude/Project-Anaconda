using AnacondaMVC.Games.WheelOfFortune;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnacondaMVC.Games;

namespace AnacondaGames.Games.WheelOfFortune
{
    public class BetMultiplier : ISpinAction
    {
        private decimal _creditsMultiplier;

        public BetMultiplier(decimal creditsMultiplier)
        {
            _creditsMultiplier = creditsMultiplier;
        }

        public GameResult Execute(GameContext context)
        {
            int gainedCredits = 0;

            if (context.Bet > 0)
            {
                gainedCredits = (int)Math.Floor(context.Bet * _creditsMultiplier);
            }

            return new GameResult() { CreditsGained = gainedCredits, Bet = context.Bet };
        }
    }
}