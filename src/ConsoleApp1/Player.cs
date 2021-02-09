using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class Player
    {
        public Player(string name, decimal chips)
        {
            Name = name;
            Chips = chips;
            Hand = new PlayerHand(this);
            SplitHands = new List<PlayerHand>();

        }

        public string Name { get; }
        public decimal Chips { get; set; }
        public PlayerHand Hand { get; private set; }
        public IList<PlayerHand> SplitHands { get; }
        public IList<PlayerHand> AllHands => new[] { Hand }.Concat(SplitHands).ToList();

        public bool IsActive => ActiveHands.Any();
        public IList<PlayerHand> ActiveHands => AllHands.Where(h => h.IsActive).ToList();
        public bool HasMultipleHands => ActiveHands.Count > 1;

        public bool IsBroke => Chips <= 0;
        public decimal BettableChips => Chips - AllHands.Sum(h => h.Wager);
        public bool HasBettableChips => BettableChips > 0;
        public bool CanSplit => HasBettableChips && SplitHands.Count < Table.MAX_SPLIT_HANDS;

        public int BlackjackCount { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public int PushCount { get; set; }
        public int HandCount => WinCount + LossCount + PushCount;

        public void Reset()
        {
            Hand = new PlayerHand(this);
        }

        public void Draw()
        {
            Console.Write($"{Name}\t{BettableChips.ToString("c")}\n");
            
            foreach (PlayerHand hand in ActiveHands)
            {
                hand.Draw();
                Console.Write("\n");
            }
        }
    }
}
