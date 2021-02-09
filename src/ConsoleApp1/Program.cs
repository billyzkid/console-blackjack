using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dealerHand = new DealerHand();
            //dealerHand.Deal(new Card(CardType.Ace, SuitType.Spades));
            //dealerHand.Deal(new Card(CardType.Jack, SuitType.Diamonds));
            //dealerHand.IsTurnPending = false;
            //dealerHand.Draw();

            //var playerHand = new PlayerHand();
            //playerHand.Deal(new Card(CardType.Five, SuitType.Spades));
            //playerHand.Deal(new Card(CardType.Six, SuitType.Diamonds));
            //playerHand.Deal(new Card(CardType.Two, SuitType.Hearts));
            //playerHand.Draw();

            var dealer = new Dealer();
            var shoe = new Shoe();
            var discardTray = new DiscardTray();
            var table = new Table(dealer, shoe, discardTray);

            table.InvitePlayers();
            table.Play();

            //var player1 = new Player("John", 500);
            //table.Players.Add(player1);

            //player1.Hands.Add(new PlayerHand(player1, 10));

            //dealer.Shuffle(shoe, discardTray);
            //dealer.Slice(shoe);

            //dealer.Burn(shoe, discardTray);

            //dealer.Deal(shoe, dealer.Hand);
            //dealer.Deal(shoe, dealer.Hand);

            //dealer.Deal(shoe, player1.Hands[0]);
            //dealer.Deal(shoe, player1.Hands[0]);

            //table.Draw();
            //Thread.Sleep(2000);

            //player1.Hands[0].Win();
            //dealer.Hand.Lose();

            //dealer.Sweep(discardTray, player1.Hands[0]);
            //dealer.Sweep(discardTray, dealer.Hand);

            //player1.Hands.Remove(player1.Hands[0]);

            //table.Draw();


            //var player = new Player("Will", 500);
            //player.Hands.Add(new PlayerHand(player, 10));
            //player.Hands[0].Deal(new Card(CardType.Six, SuitType.Diamonds));
            //player.Hands[0].Deal(new Card(CardType.Six, SuitType.Spades));
            //player.Hands[0].Split();
            //player.Draw();

            //var table = new Table(new Dealer(), new Shoe(Table.SHOE_SIZE), new DiscardTray(), new List<Player>());

            //InvitePlayers(table);

            //while (table.HasPlayers)
            //{
            //    BootBrokePlayers(table);

            //    if (table.HasPlayers)
            //    {
            //        PlaceBets(table);

            //        Deal(table);
            //    }
            //}
        }

        //private static void InvitePlayers(Table table)
        //{
        //    table.Draw();

        //    SitPlayer(table);

        //    while (table.Players.Count < Table.MAX_SEATS)
        //    {
        //        table.Draw();

        //        Console.Write("Are more players joining us today? ");
        //        var key = Console.ReadKey();

        //        if ("yY".Contains(key.KeyChar))
        //        {
        //            table.Draw();

        //            SitPlayer(table);

        //            if (table.Players.Count >= Table.MAX_SEATS)
        //            {
        //                Console.Write("Table is full. Let's play!");
        //                Thread.Sleep(2500);
        //            }
        //        }
        //        else if ("nN".Contains(key.KeyChar))
        //        {
        //            break;
        //        }
        //    }

        //    table.Draw();
        //}

        //private static void SitPlayer(Table table)
        //{
        //    Console.Write("Welcome to the casino, what's your name? ");
        //    var name = Console.ReadLine();

        //    Console.Write($"Hello {name}, what is your bankroll today? ");
        //    var bankroll = int.Parse(Console.ReadLine());

        //    var player = new Player(name, bankroll);

        //    table.Sit(player);
        //}

        //private static void BootBrokePlayers(Table table)
        //{
        //    foreach (Player brokePlayer in table.BrokePlayers)
        //    {
        //        Console.Write($"{brokePlayer.Name}, looks like you are broke. Better luck next time.\n");
        //        table.Leave(brokePlayer);
        //    }

        //    table.Draw();
        //}

        //private static void PlaceBets(Table table)
        //{
        //    foreach (Player player in table.Players)
        //    {
        //        while (true)
        //        {
        //            Console.Write($"{player.Name}, what's your bet? ");
        //            var amount = int.Parse(Console.ReadLine());

        //            if (amount <= 0)
        //            {
        //                break;
        //            }
        //            else if (amount > player.Chips)
        //            {
        //                Console.Write($"Please bet within your bankroll: {player.Chips}");
        //            }
        //            else if (amount < Table.MIN_BET || amount > Table.MAX_BET)
        //            {
        //                Console.Write($"Please bet within table limits: ${Table.MIN_BET} - ${Table.MAX_BET}");
        //            }
        //            else
        //            {
        //                break;
        //            }

        //            player.Bet(amount);
        //        }
        //    }
        //}

        //private static void Deal(Table table)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
