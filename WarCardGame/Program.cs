using System;
using System.Linq;

namespace WarCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Player #1");
            var player1Deck = GetDeckInput();

            Console.WriteLine("\r\nPlayer #2");
            var player2Deck = GetDeckInput();

#if DEBUG
            DisplayDecks(player1Deck, player2Deck);
#endif

            try {
                var game = new Game(player1Deck, player2Deck);
                int winner = game.PlayToFinish();

                Console.WriteLine("\r\n\r\nPlayer {0} wins", winner);
                Console.WriteLine("Rounds Taken: {0}", game.RoundsPlayed);
                Console.WriteLine("Wars Played: {0}", game.WarsPlayed);
            }
            catch (ApplicationException ex) {
                Console.WriteLine("\r\n\r\n" + ex.Message);
            }
        }

        static Deck GetDeckInput()
        {
            Console.WriteLine("Input the deck (each card is a number 1-13 separated by a space");
            string deckInput = Console.ReadLine().Trim();

            try {
                var inputValues = deckInput.Split(' ');
                var cards = from value in inputValues
                            let cardValue = Convert.ToUInt16(value)
                            select cardValue;

                return new Deck(cards);
            }
            catch (FormatException) {
                Console.WriteLine("Invalid input for deck");
                Environment.Exit(0);
                return null;
            }
        }

        static void DisplayDecks(Deck player1, Deck player2)
        {
            Console.Write("\r\n\r\nPlayer #1: ");
            DisplayDeck(player1);
            Console.Write("\r\n\r\nPlayer #2: ");
            DisplayDeck(player2);
        }

        static void DisplayDeck(Deck deck)
        {
            foreach (var card in deck.GetCards()) {
                Console.Write(card.ToString().PadLeft(2) + " ");
            }

            Console.WriteLine();
        }
    }
}
