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
        public List<SlotItem> GetRandomColumnFruit()
        {
            var items = new List<RandomItem<SlotItem>>
            {
                new RandomItem<SlotItem>(0.15, new SlotItem("Banana", 0.5)),
                new RandomItem<SlotItem>(0.15, new SlotItem("Apple", 0.4)),
                new RandomItem<SlotItem>(0.1, new SlotItem("Gold", 1)),
                new RandomItem<SlotItem>(0.2, new SlotItem("Kiwi", 0.25)),
                new RandomItem<SlotItem>(0.2, new SlotItem("Raspberry", 0.2)),
                new RandomItem<SlotItem>(0.2, new SlotItem("Strawberry", 0.15))
            };
            Random rand = new Random();
            var randomPicker = new RandomPicker<SlotItem>(items, rand.Next());

            List<SlotItem> slotItems = new List<SlotItem>();

            for (int i = 0; i < 3; i++)
            {
                slotItems.Add(randomPicker.Pick().Item);
            }

            return slotItems;
        }
    }
}