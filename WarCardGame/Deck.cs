using System.Collections.Generic;
using System.Linq;

namespace WarCardGame
{
    public sealed class Deck
    {
        readonly Queue<ushort> cards;

        public Deck(IEnumerable<ushort> initCards)
            => cards = new Queue<ushort>(initCards);

        public bool HasCards
            => (cards.Count > 0);

        public int CardCount
            => cards.Count;

        public IEnumerable<ushort> GetCards()
            => cards.AsEnumerable();

        public ushort TakeCard()
            => cards.Dequeue();

        public ushort[] TakeCards(int count)
        {
            var taken = new ushort[count];

            for (int i = 0; i < count; i++) {
                taken[i] = cards.Dequeue();
            }

            return taken;
        }

        public void AddCards(IEnumerable<ushort> newCards)
        {
            foreach (var card in newCards) {
                cards.Enqueue(card);
            }
        }
    }
}
