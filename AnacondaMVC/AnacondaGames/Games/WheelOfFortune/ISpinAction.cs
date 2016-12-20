using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaMVC.Games.WheelOfFortune
{
    public interface ISpinAction
    {
        GameResult Execute(GameContext context);
    }
}