using System.Security.Cryptography.X509Certificates;
using AnacondaGames.Games.Roulette;
using AnacondaMVC.Games;

namespace AnacondaGames.Games
{
    public interface ISingleBetGame<TGameResult> : IGame where TGameResult : GameResult
    {

        TGameResult Play(Bet bet);

    }
}