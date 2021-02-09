using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class Dealer
    {
        private readonly Random random = new Random();

        public Dealer()
        {
            Hand = new DealerHand(this);
        }

        public DealerHand Hand { get; private set; }

        public int BlackjackCount { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public int PushCount { get; set; }
        public int HandCount => WinCount + LossCount + PushCount;

        public void Shuffle(Shoe shoe, DiscardTray discardTray)
        {
            var allCards = shoe.Cards.Concat(discardTray.Cards).ToList();
            var shuffledCards = new List<Card>();

            while (allCards.Any())
            {
                var index = random.Next(allCards.Count);
                shuffledCards.Add(allCards.ElementAt(index));
                allCards.RemoveAt(index);
            }

            shoe.Reload(shuffledCards);
        }

        public void Slice(Shoe shoe)
        {
            shoe.SliceLocation = random.Next(Table.MIN_SLICE, Table.MAX_SLICE);
        }

        public void Burn(Shoe shoe, DiscardTray discardTray)
        {
            discardTray.Cards.Add(shoe.NextCard());
        }

        public void Deal(Shoe shoe, Hand hand)
        {
            hand.Cards.Add(shoe.NextCard());
        }

        public void Sweep(DiscardTray discardTray, Hand hand)
        {
            foreach (Card card in hand.Cards)
            {
                discardTray.Cards.Add(card);
            }

            hand.Cards.Clear();
        }

        public void Draw()
        {
            Console.Write("Dealer\t");
            Hand.Draw();
        }

        public void Reset()
        {
            Hand = new DealerHand(this);
        }
    }
}
