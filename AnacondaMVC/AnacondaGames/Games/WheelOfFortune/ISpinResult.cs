using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaMVC.Games.WheelOfFortune
{
    interface ISpinResult
    {
        GameResult Execute(GameContext context);
    }
}