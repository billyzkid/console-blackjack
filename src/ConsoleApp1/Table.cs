using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class Table
    {
        public const int MAX_SEATS = 2;
        public const int SHOE_SIZE = 6;
        public const int MIN_SLICE = 50;
        public const int MAX_SLICE = 100;
        public const int MAX_SPLIT_HANDS = 3;
        public const int DEALER_HARD_STAND_VALUE = 17;
        public const int DEALER_SOFT_STAND_VALUE = 18;
        public const decimal MIN_BET = 5;
        public const decimal MAX_BET = 500;
        public const decimal BLACKJACK_PAYOUT = 1.5m;

        public Table(Dealer dealer, Shoe shoe, DiscardTray discardTray)
        {
            Dealer = dealer;
            Shoe = shoe;
            DiscardTray = discardTray;
            Players = new List<Player>();
        }

        public Dealer Dealer { get; }
        public Shoe Shoe { get; }
        public DiscardTray DiscardTray { get; }
        public IList<Player> Players { get; }

        public IList<Player> BrokePlayers => Players.Where(p => p.IsBroke).ToList();
        public IList<Player> BettablePlayers => Players.Where(p => p.Chips >= MIN_BET).ToList();
        public IList<Player> ActivePlayers => Players.Where(p => p.IsActive).ToList();
        public IList<PlayerHand> ActivePlayerHands => ActivePlayers.SelectMany(p => p.ActiveHands).ToList();
        public IList<Player> ActivePlayersWithBlackjack => ActivePlayers.Where(p => p.Hand.IsBlackjack).ToList();

        public bool HasPlayers => Players.Any();
        public bool HasActivePlayers => ActivePlayers.Any();
        public bool HasOpenSeats => Players.Count < MAX_SEATS;
        private bool HasPlayStarted { get; set; }

        public void InvitePlayers()
        {
            AddPlayer();

            while (HasOpenSeats)
            {
                Console.Write("Are more players joining us today? ");
                var key = Console.ReadKey();

                if ("yY".Contains(key.KeyChar))
                {
                    AddPlayer();

                    if (!HasOpenSeats)
                    {
                        Console.Write("Table is full. Let's play!");
                        Thread.Sleep(2500);
                        Draw();
                    }
                }
                else if ("nN".Contains(key.KeyChar))
                {
                    Draw();
                    break;
                }
            }
        }

        public bool AskSplit(Player player)
        {
            while (true)
            {
                Console.Write($"{player.Name}, would you like to split? ");
                var key = Console.ReadKey();

                if ("yY".Contains(key.KeyChar))
                {
                    return true;
                }
                else if ("nN".Contains(key.KeyChar))
                {
                    return false;
                }
            }
        }

        public Action PlayerFirst(Player player)
        {
            var action = Action.None;

            if (player.HasBettableChips)
            {
                while (action == Action.None)
                {
                    Console.Write($"{player.Name}, what's your play? Hit, stand, or double? ");
                    var key = Console.ReadKey().KeyChar;

                    if ("hH".Contains(key))
                    {
                        action = Action.Hit;
                    }
                    else if ("sS".Contains(key))
                    {
                        action = Action.Stand;
                    }
                    else if ("dD".Contains(key))
                    {
                        action = Action.Double;
                    }
                }
            }
            else
            {
                action = PlayerNext(player);
            }

            return action;
        }

        public Action PlayerNext(Player player)
        {
            var action = Action.None;

            while (action == Action.None)
            {
                Console.Write($"{player.Name}, what's your play? Hit or stand? ");
                var key = Console.ReadKey().KeyChar;

                if ("hH".Contains(key))
                {
                    action = Action.Hit;
                }
                else if ("sS".Contains(key))
                {
                    action = Action.Stand;
                }
            }

            return action;
        }

        private void AddPlayer()
        {
            Draw();

            Console.Write("Welcome to the casino! What's your name? ");
            var name = Console.ReadLine();

            Console.Write($"Hello {name}. What is your bankroll today? ");
            var chips = int.Parse(Console.ReadLine());

            var player = new Player(name, chips);
            Players.Add(player);

            Draw();
        }

        public void Play()
        {
            while (true)
            {
                PlaceBets();

                if (!HasPlayStarted || Shoe.IsEmpty)
                {
                    Console.Write("Shuffling...");
                    Thread.Sleep(2000);

                    Dealer.Shuffle(Shoe, DiscardTray);
                    Dealer.Slice(Shoe);
                    Dealer.Burn(Shoe, DiscardTray);
                }

                HasPlayStarted = true;

                Deal();
                Deal();

                if (Dealer.Hand.IsPossibleBlackjack)
                {
                    Console.WriteLine("Checking for dealer blackjack.");
                    Thread.Sleep(2000);

                    if (Dealer.Hand.IsBlackjack)
                    {
                        Dealer.Hand.IsTurnPending = false;
                        Draw();

                        Console.WriteLine("Dealer has blackjack.");
                        Thread.Sleep(2000);

                        FinishHands();
                    }
                    else
                    {
                        Console.WriteLine("Dealer does not have blackjack.");
                        Thread.Sleep(2000);

                        PlayHands();
                    }
                }
                else
                {
                    PlayHands();
                }

                if (HasActivePlayers)
                {
                    PlayHand(Dealer.Hand);
                    FinishHands();
                }
                else
                {
                    Dealer.Sweep(DiscardTray, Dealer.Hand);
                }
            }
        }

        private void FinishHands()
        {
            foreach (var playerHand in ActivePlayerHands)
            {
                FinishHand(playerHand, Dealer.Hand);
                Thread.Sleep(2000);
            }

            Dealer.Sweep(DiscardTray, Dealer.Hand);
            Dealer.Reset();
            Thread.Sleep(2000);
        }

        private void FinishHand(PlayerHand playerHand, DealerHand dealerHand)
        {
            var outcome = CalculateOutcome(playerHand, dealerHand);

            if (outcome == Outcome.Win)
            {
                Console.WriteLine($"{playerHand.Player.Name}, you won!");
                playerHand.Win();
                dealerHand.Lose();
            }
            else if (outcome == Outcome.Lose)
            {
                Console.WriteLine($"{playerHand.Player.Name}, you lost.");
                playerHand.Lose();
                dealerHand.Win();
            }
            else
            {
                Console.WriteLine($"{playerHand.Player.Name}, you pushed.");
                playerHand.Push();
                dealerHand.Push();
            }

            Thread.Sleep(2000);

            dealerHand.Dealer.Sweep(DiscardTray, playerHand);
            
            Draw();
        }

        private Outcome CalculateOutcome(PlayerHand playerHand, DealerHand dealerHand)
        {
            var player = playerHand.FinalValue;
            var dealer = dealerHand.FinalValue;

            if (dealerHand.IsBlackjack)
            {
                return playerHand.IsBlackjack ? Outcome.Push : Outcome.Lose;
            }
            else if (dealerHand.IsBusted)
            {
                return Outcome.Win;
            }
            else if (player > dealer)
            {
                return Outcome.Win;
            }
            else if (player < dealer)
            {
                return Outcome.Lose;
            }
            else
            {
                return Outcome.Push;
            }
        }

        private void PlayHands()
        {
            foreach (var activePlayer in ActivePlayersWithBlackjack)
            {
                PayBlackjack(activePlayer.Hand);
            }

            foreach (var activePlayer in ActivePlayers)
            {
                PlayHand(activePlayer.Hand);
            }
        }

        private void PlayHand(PlayerHand hand)
        {
            if (hand.Player.ActiveHands.Count > 1)
            {
                ShowCurrentHand(hand, hand.Player);
            }

            var action = Action.None;

            if (hand.CanSplit && hand.Player.CanSplit && AskSplit(hand.Player))
            {
                action = Action.Split;
            }
            else
            {
                action = PlayerFirst(hand.Player);
            }

            Draw();

            if (action == Action.Hit)
            {
                PlayHandUntilDone(hand);
            }
            else if (action == Action.Double)
            {
                DoubleDown(hand);
            }
            else if (action == Action.Split)
            {
                Split(hand);
            }
            else if (action == Action.Stand)
            {
                hand.Stand();
                Draw();
            }
        }

        private void PlayHandUntilDone(PlayerHand hand)
        {
            Hit(hand);

            if (hand.Player.HasMultipleHands)
            {
                ShowCurrentHand(hand, hand.Player);
            }

            var action = Action.None;
            while (!hand.IsBusted && action != Action.Stand)
            {
                action = PlayerNext(hand.Player);
                if (action == Action.Hit)
                {
                    Hit(hand);
                }
            }
            if (action == Action.Stand)
            {
                hand.Stand();
                Draw();
            }
            else if (hand.IsBusted)
            {
                Bust(hand);
            }
        }

        private void DoubleDown(PlayerHand hand)
        {
            var doubleBet = Math.Min(hand.Wager, hand.Player.BettableChips);
            Console.WriteLine($"Doubling down for ${doubleBet}");
            hand.Bet(doubleBet);
            Thread.Sleep(2000);

            Dealer.Deal(Shoe, hand);
            Draw();
            if (hand.IsBusted)
            {
                Bust(hand);
            }
            else
            {
                hand.Double();
                Draw();
            }
        }

        private void Split(PlayerHand hand)
        {
            var splitHand = hand.Split();
            Draw();
            Dealer.Deal(Shoe, hand);
            Draw();
            Dealer.Deal(Shoe, splitHand);
            Draw();
            PlayHand(hand);
            PlayHand(splitHand);
        }

        private void ShowCurrentHand(PlayerHand hand, Player player)
        {
            Console.WriteLine($"\nCurrent hand for {player.Name}");
            hand.Draw();
            Console.WriteLine();
        }

        private void Hit(PlayerHand hand)
        {
            Dealer.Deal(Shoe, hand);
            Draw();
        }

        private void Bust(PlayerHand hand)
        {
            hand.Lose();
            Dealer.Hand.Win();
            Dealer.Sweep(DiscardTray, hand);
            Thread.Sleep(2000);
            Draw();

            if (!HasActivePlayers)
            {
                
                Draw();
                Thread.Sleep(2000);
                Dealer.Sweep(DiscardTray, Dealer.Hand); //sweep own hand
                Dealer.Reset();
                Draw();
                Thread.Sleep(2000);
            }
        }

        private void PlayHand(DealerHand hand)
        {
            hand.IsTurnPending = false;
            Draw();

            if (HasActivePlayers)
            {
                while (hand.KeepPlaying)
                {
                    Dealer.Deal(Shoe, hand);
                    Draw();
                }
            }

            Thread.Sleep(2000);
        }

        private void PayBlackjack(PlayerHand playerHand)
        {
            playerHand.Blackjack();
            Thread.Sleep(2000);
            Dealer.Sweep(DiscardTray, playerHand);
            Draw();
        }

        private void Deal()
        {
            foreach (var activePlayer in ActivePlayers)
            {
                Dealer.Deal(Shoe, activePlayer.Hand);
                Draw();
            }

            Dealer.Deal(Shoe, Dealer.Hand);
            Draw();
        }

        private void PlaceBets()
        {
            foreach (Player player in BettablePlayers)
            {
                while (true)
                {
                    Console.Write($"{player.Name}, what's your bet? ");
                    var bet = int.Parse(Console.ReadLine());

                    if (bet == 0)
                    {
                        break;
                    }
                    else if (bet > player.Chips)
                    {
                        Console.Write($"Please bet within your bankroll: {player.Chips}\n");
                    }
                    else if (bet < MIN_BET || bet > MAX_BET)
                    {
                        Console.Write($"Please bet within table limits: {MIN_BET.ToString("c")} - {MAX_BET.ToString("c")}\n");
                    }
                    else
                    {
                        player.Hand.Bet(bet);
                        Draw();
                        break;
                    }
                }
            }
        }

        public void Draw()
        {
            Console.Clear();
            Console.Write($"Blackjack {MIN_BET.ToString("c")} to {MAX_BET.ToString("c")}\n\n");

            Dealer.Draw();
            
            foreach (Player player in Players)
            {
                Console.Write("\n\n");

                player.Draw();
            }

            Console.Write("\n\n---------------------------------\n\n");
        }
    }
}
