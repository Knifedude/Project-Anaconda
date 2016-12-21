using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaGames.Games.SlotMachine
{
    public class SlotItem
    {
        public string DisplayName { get; set; }
        public double Value { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="value">Value between 0.0 and 1.0</param>
        public SlotItem(string displayName, double value)
        {
            DisplayName = displayName;
            Value = value;
        }
    }
}