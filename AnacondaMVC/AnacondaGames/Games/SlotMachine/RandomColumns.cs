using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnacondaGames.Games.WheelOfFortune;

namespace AnacondaGames.Games.SlotMachine
{
    public class RandomColumns
    {
        public void GetRandomColumnFruit()
        {
            var items = new List<RandomItem<SlotItem>>
            {
                new RandomItem<SlotItem>(0.6, new SlotItem("Banana", 0.2)),
                new RandomItem<SlotItem>(0.2, new SlotItem("Apple", 0.2)),
                new RandomItem<SlotItem>(0.1, new SlotItem("Gold", 0.2)),
                new RandomItem<SlotItem>(0.1 / 3, new SlotItem("Kiwi", 0.2)),
                new RandomItem<SlotItem>(0.1 / 3, new SlotItem("Raspberry", 0.2)),
                new RandomItem<SlotItem>(0.1 / 3, new SlotItem("Strawberry", 0.2))
            };
            Random rand = new Random();
            var randomPicker = new RandomPicker<SlotItem>(items, rand.Next());
        }
    }
}