using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public sealed class Shoe
    {
        public Shoe()
        {
            Decks = Enumerable.Range(1, Table.SHOE_SIZE).Select(i => new Deck()).ToList();
            Cards = Decks.SelectMany(d => d.Cards).ToList();
        }

        public IList<Deck> Decks { get; }
        public IList<Card> Cards { get; }

        public int SliceLocation { get; set; }
        public bool IsEmpty => Cards.Count <= SliceLocation;

        public Card NextCard()
        {
            if (IsEmpty)
            {
                throw new Exception("Shoe is empty");
            }

            var card = Cards.ElementAt(0);
            Cards.RemoveAt(0);
            return card;
        }

        public void Reload(IList<Card> cards)
        {
            Cards.Clear();
            
            foreach (var card in cards)
            {
                Cards.Add(card);
            }
        }

        public void Draw()
        {
            foreach (Card card in Cards)
            {
                card.Draw();
                Console.Write(" ");
            }
        }
    }
}
