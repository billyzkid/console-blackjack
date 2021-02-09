using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class DealerHand : Hand
    {
        public DealerHand(Dealer dealer)
        {
            Dealer = dealer;
        }

        public Dealer Dealer { get; }

        public bool IsTurnPending { get; set; } = true;

        public Card UpCard => FirstCard;
        public Card DownCard => SecondCard;
        public bool HasUpCard => FirstCard != null;
        public bool HasDownCard => SecondCard != null;

        public bool IsPossibleBlackjack => HasBeenDealt && (UpCard?.IsAce == true || UpCard?.HasValueOfTen == true);
        public bool KeepPlayingSoft => HasSoftValue && SoftHighValue < Table.DEALER_SOFT_STAND_VALUE;
        public bool KeepPlayingHard => !HasSoftValue && HardValue < Table.DEALER_HARD_STAND_VALUE;
        public bool KeepPlaying => !IsBusted && (KeepPlayingSoft || KeepPlayingHard);

        public string Status => IsBusted ? "BUSTED" : string.Empty;

        public override void Blackjack()
        {
            Outcome = Outcome.Blackjack;
            Dealer.BlackjackCount++;
            Dealer.WinCount++;
        }

        public override void Win()
        {
            Outcome = Outcome.Win;
            Dealer.WinCount++;
        }

        public override void Lose()
        {
            Outcome = Outcome.Lose;
            Dealer.LossCount++;
        }

        public override void Push()
        {
            Outcome = Outcome.Push;
            Dealer.PushCount++;
        }

        public override void Draw()
        {
            if (IsTurnPending)
            {
                if (HasUpCard)
                {
                    UpCard.Draw();
                }

                Console.Write(" ");

                if (HasDownCard)
                {
                    DownCard.Draw(hide: true);
                }
            }
            else if (HasCards)
            {
                foreach (Card card in Cards)
                {
                    card.Draw();
                    Console.Write(" ");
                }

                if (HasBeenDealt)
                {
                    if (IsBlackjack)
                    {
                        Console.Write("BLACKJACK!");
                    }
                    else if (HasSoftValue)
                    {
                        Console.Write($"({SoftLowValue} or {SoftHighValue})\t{Status}");
                    }
                    else
                    {
                        Console.Write($"({FinalValue})\t{Status}");
                    }
                }
            }
        }
    }
}
