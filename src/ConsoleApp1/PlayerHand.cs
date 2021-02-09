using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class PlayerHand : Hand
    {
        public PlayerHand(Player player)
        {
            Player = player;
        }

        public Player Player { get; }

        public decimal Wager { get; private set; }
        public bool HasWager => Wager > 0;

        public bool IsDoubled { get; private set; }
        public bool IsStood { get; private set; }
        public bool IsSplit { get; private set; }

        public bool CanSplit => IsFirstTurn && FirstCard.Type == SecondCard.Type && Wager <= Player.BettableChips;
        public string Status => IsBusted ? "BUSTED" : IsDoubled ? "DOUBLED" : IsStood ? "STOOD" : string.Empty;

        public void Bet(decimal chips)
        {
            Wager += chips;
        }

        public void Double()
        {
            IsDoubled = true;
        }

        public void Stand()
        {
            IsStood = true;
        }

        public PlayerHand Split()
        {
            IsSplit = true;

            var splitHand = new PlayerHand(Player);
            splitHand.Bet(Wager);
            splitHand.Cards.Add(SecondCard);
            Cards.Remove(SecondCard);
            Player.SplitHands.Add(splitHand);

            return splitHand;
        }

        public override void Blackjack()
        {
            Outcome = Outcome.Blackjack;
            Player.BlackjackCount++;
            Player.WinCount++;
            Player.Chips += Wager * Table.BLACKJACK_PAYOUT;
            Eliminate();
        }

        public override void Win()
        {
            Outcome = Outcome.Win;
            Player.WinCount++;
            Player.Chips += Wager;
            Eliminate();
        }

        public override void Lose()
        {
            Outcome = Outcome.Lose;
            Player.LossCount++;
            Player.Chips -= Wager;
            Eliminate();
        }

        public override void Push()
        {
            Outcome = Outcome.Push;
            Player.PushCount++;
            Eliminate();
        }

        private void Eliminate()
        {
            if (Player.SplitHands.Contains(this))
            {
                Player.SplitHands.Remove(this);
            }
            else
            {
                Player.Reset();
            }
        }

        public override void Draw()
        {
            if (HasWager)
            {
                Console.Write($"{Wager.ToString("c")}\t");
            }
            
            if (HasCards)
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
