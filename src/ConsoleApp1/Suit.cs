using System;

namespace ConsoleApp1
{
    public sealed class Suit
    {
        public Suit(SuitType type)
        {
            Type = type;
        }

        public SuitType Type { get; }
        public char Symbol => GetSymbol(Type);
        public ConsoleColor Color => GetColor(Type);

        private static char GetSymbol(SuitType type)
        {
            switch (type)
            {
                case SuitType.Diamonds:
                    return '♦';
                case SuitType.Hearts:
                    return '♥';
                case SuitType.Clubs:
                    return '♣';
                case SuitType.Spades:
                    return '♠';
                default:
                    throw new ArgumentException("Invalid type.", nameof(type));
            }
        }

        private static ConsoleColor GetColor(SuitType type)
        {
            switch (type)
            {
                case SuitType.Diamonds:
                case SuitType.Hearts:
                    return ConsoleColor.Red;
                case SuitType.Clubs:
                case SuitType.Spades:
                    return ConsoleColor.Black;
                default:
                    throw new ArgumentException("Invalid type.", nameof(type));
            }
        }

        public void Draw()
        {
            Console.Write(Symbol);
        }
    }

    public enum SuitType
    {
        Diamonds,
        Hearts,
        Clubs,
        Spades
    }
}
