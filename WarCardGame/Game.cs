using System;
using System.Collections.Generic;
using System.Linq;

namespace WarCardGame
{
    public sealed class Game
    {
        readonly Deck[] playDecks;
        int rounds, wars;

        public Game(Deck player1, Deck player2) 
            => playDecks = new[] { player1, player2 };

        public int RoundsPlayed
            => rounds;

        public int WarsPlayed
            => wars;

        public int PlayToFinish()
        {
            while (playDecks.All(d => d.HasCards)) {
                PlayRound();
                ++rounds;
            }

            return (playDecks[0].HasCards ? 1 : 2);
        }

        void PlayRound()
        {
            var playedCards = new[] { playDecks[0].TakeCard(), playDecks[1].TakeCard() };
            int winnerIndex;

            if (playedCards[0] == playedCards[1]) {
                var warCards = new List<ushort>();
                winnerIndex = War(warCards);

                playDecks[winnerIndex].AddCards(warCards);
            }
            else {
                winnerIndex = CalculateWinner(playedCards);
            }

            playDecks[winnerIndex].AddCards(playedCards);
        }

        int War(List<ushort> cards)
        {
            int takeCount = CalculateWarCardCount();
            int winnerIndex;

            if (takeCount > 0) {
                var cardsInPlay = new[] { playDecks[0].TakeCards(takeCount), playDecks[1].TakeCards(takeCount) };
                ushort player1UpCard = cardsInPlay[0][takeCount - 1];
                ushort player2UpCard = cardsInPlay[1][takeCount - 1];

                if (player1UpCard == player2UpCard) {
                    winnerIndex = War(cards);
                }
                else {
                    winnerIndex = CalculateWinner(new[] { player1UpCard, player2UpCard });
                }

                // ensures winner cards are added back to the deck first
                cards.AddRange(cardsInPlay[winnerIndex]);
                cards.AddRange(cardsInPlay[Math.Abs(winnerIndex - 1 )]);
            }
            else {
                winnerIndex = CalculateWinner(playDecks);
            }

            ++wars;
            return winnerIndex;
        }

        static int CalculateWinner(ushort[] playedCards)
            => (playedCards[0] > playedCards[1] ? 0 : 1);

        static int CalculateWinner(Deck[] playDecks)
        {
            bool player1HasCards = playDecks[0].HasCards;
            bool player2HasCards = playDecks[1].HasCards;

            if (player1HasCards == player2HasCards) {
                throw new ApplicationException("Game ends in a tie.");
            }
            else {
                return (player1HasCards ? 0 : 1);
            }
        }

        int CalculateWarCardCount()
        {
            var maxCards = Math.Min(playDecks[0].CardCount, playDecks[1].CardCount);
            const int DesiredCardCount = 4;

            return (maxCards >= DesiredCardCount ? DesiredCardCount : maxCards);
        }
    }
}
