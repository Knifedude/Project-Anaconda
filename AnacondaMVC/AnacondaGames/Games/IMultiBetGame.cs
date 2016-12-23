using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnacondaGames.Games.Roulette;

namespace AnacondaGames.Games
{
    public interface IMultiBetGame : IGame
    {

        void Play(ICollection<Bet> bets);

    }
}
