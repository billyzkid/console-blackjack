using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public sealed class Deck
    {
        public Deck()
        {
            Cards = new List<Card>(52);

            var suits = new List<Suit>
            {
                new Suit(SuitType.Diamonds),
                new Suit(SuitType.Hearts),
                new Suit(SuitType.Clubs),
                new Suit(SuitType.Spades)
            };

            suits.ForEach(suit =>
            {
                Cards.Add(new Card(CardType.Ace, suit));
                Cards.Add(new Card(CardType.Two, suit));
                Cards.Add(new Card(CardType.Three, suit));
                Cards.Add(new Card(CardType.Four, suit));
                Cards.Add(new Card(CardType.Five, suit));
                Cards.Add(new Card(CardType.Six, suit));
                Cards.Add(new Card(CardType.Seven, suit));
                Cards.Add(new Card(CardType.Eight, suit));
                Cards.Add(new Card(CardType.Nine, suit));
                Cards.Add(new Card(CardType.Ten, suit));
                Cards.Add(new Card(CardType.Jack, suit));
                Cards.Add(new Card(CardType.Queen, suit));
                Cards.Add(new Card(CardType.King, suit));
            });
        }

        public IList<Card> Cards { get; }

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
