using System;

namespace ConsoleApp1
{
    public sealed class Card
    {
        public Card(CardType type, SuitType suitType) : this(type, new Suit(suitType))
        {
        }

        public Card(CardType type, Suit suit)
        {
            Type = type;
            Suit = suit;
        }

        public CardType Type { get; }
        public Suit Suit { get; }
        public string Token => GetToken(Type);
        public int Value => GetValue(Type);

        public bool IsAce => Type == CardType.Ace;
        public bool HasValueOfTen => Value == 10;

        private static string GetToken(CardType type)
        {
            switch (type)
            {
                case CardType.Ace:
                    return "A";
                case CardType.Two:
                    return "2";
                case CardType.Three:
                    return "3";
                case CardType.Four:
                    return "4";
                case CardType.Five:
                    return "5";
                case CardType.Six:
                    return "6";
                case CardType.Seven:
                    return "7";
                case CardType.Eight:
                    return "8";
                case CardType.Nine:
                    return "9";
                case CardType.Ten:
                    return "10";
                case CardType.Jack:
                    return "J";
                case CardType.Queen:
                    return "Q";
                case CardType.King:
                    return "K";
                default:
                    throw new ArgumentException("Invalid type.", nameof(type));
            }
        }

        private static int GetValue(CardType type)
        {
            switch (type)
            {
                case CardType.Ace:
                    return 1;
                case CardType.Two:
                    return 2;
                case CardType.Three:
                    return 3;
                case CardType.Four:
                    return 4;
                case CardType.Five:
                    return 5;
                case CardType.Six:
                    return 6;
                case CardType.Seven:
                    return 7;
                case CardType.Eight:
                    return 8;
                case CardType.Nine:
                    return 9;
                case CardType.Ten:
                case CardType.Jack:
                case CardType.Queen:
                case CardType.King:
                    return 10;
                default:
                    throw new ArgumentException("Invalid type.", nameof(type));
            }
        }

        public void Draw(bool hide = false)
        {
            if (hide)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("  ");
                Console.ResetColor();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = Suit.Color;
                Console.Write(Token);
                Suit.Draw();
                Console.ResetColor();
            }
        }
    }

    public enum CardType
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
}
