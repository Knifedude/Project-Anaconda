using AnacondaMVC.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaGames.Games.SlotMachine
{
    public class CheckRows
    {
        public GameResult WinResult (List<SlotItem>[] slotColumns, GameContext context)
        {
            double value = 0.0;
            double winMultiplier = 5.0;

            for (int i = 0; i < 3; i++)
            {
                if (slotColumns[i][0].DisplayName == slotColumns[i][1].DisplayName && slotColumns[i][0].DisplayName == slotColumns[i][2].DisplayName)
                {
                    if (slotColumns[i][0].Value > value)
                    {
                        value = slotColumns[i][0].Value;
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (slotColumns[0][i].DisplayName == slotColumns[1][i].DisplayName && slotColumns[0][i].DisplayName == slotColumns[2][i].DisplayName)
                {
                    if (slotColumns[0][i].Value > value)
                    {
                        value = slotColumns[0][i].Value;
                    }
                }
            }

            if (slotColumns[0][0].DisplayName == slotColumns[1][1].DisplayName && slotColumns[0][0].DisplayName == slotColumns[2][2].DisplayName)
            {
                if (slotColumns[0][0].Value > value)
                {
                    value = slotColumns[0][0].Value;
                }
            }

            if (slotColumns[2][0].DisplayName == slotColumns[1][1].DisplayName && slotColumns[2][0].DisplayName == slotColumns[0][2].DisplayName)
            {
                if (slotColumns[2][0].Value > value)
                {
                    value = slotColumns[2][0].Value;
                }
            }

            GameResult result = new GameResult();
            result.Bet = context.Bet;
            if (value > 0)
            {
                result.CreditsGained = (int)(context.Bet * (1 + value * winMultiplier));
            }
            else
            {
                result.CreditsGained = 0;
            }

            return result;
        }
    }
}