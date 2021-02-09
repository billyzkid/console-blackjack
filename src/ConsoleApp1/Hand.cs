using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public abstract class Hand
    {
        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = 21;
        private const int ACE_LOW_VALUE = 1;
        private const int ACE_HIGH_VALUE = 11;

        public Hand()
        {
            Cards = new List<Card>();
        }

        public IList<Card> Cards { get; }
        public IList<Card> HardCards => Cards.Where(c => !c.IsAce).ToList();
        public IList<Card> Aces => Cards.Where(c => c.IsAce).ToList();

        public bool HasAces => Aces.Any();
        public bool HasCards => Cards.Any();
        public bool HasBeenDealt => Cards.Count > 1;
        public bool IsFirstTurn => Cards.Count == 2;

        public Card FirstCard => Cards.ElementAtOrDefault(0);
        public Card SecondCard => Cards.ElementAtOrDefault(1);

        public int HardValue => HardCards.Sum(c => c.Value);
        public int SoftLowValue => HasAces ? ACE_LOW_VALUE * Aces.Count + HardValue : MIN_VALUE;
        public int SoftHighValue => HasAces ? ACE_HIGH_VALUE + ACE_LOW_VALUE * (Aces.Count - 1) + HardValue : MIN_VALUE;
        public bool HasSoftValue => HasAces && SoftHighValue <= MAX_VALUE;
        public int FinalValue => HasSoftValue ? SoftHighValue : HasAces ? SoftLowValue : HardValue;

        public bool IsBusted => FinalValue > MAX_VALUE;
        public bool IsBlackjack => (FirstCard?.IsAce == true && SecondCard?.HasValueOfTen == true) || (FirstCard?.HasValueOfTen == true && SecondCard?.IsAce == true);

        public Outcome Outcome { get; protected set; } = Outcome.Pending;
        public bool IsActive => Outcome == Outcome.Pending;

        public abstract void Blackjack();
        public abstract void Win();
        public abstract void Lose();
        public abstract void Push();
        public abstract void Draw();
    }

    public enum Outcome
    {
        Pending,
        Blackjack,
        Win,
        Lose,
        Push
    }

    public enum Action
    {
        None,
        Hit,
        Stand,
        Double,
        Split
    }
}
