using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare
{
    internal class CardDeck
    {
        public List<Card> deck { get; protected set; }

        public CardDeck()
        {
            MakeDeck();
        }

        public Card Peek()
        {
            return deck[deck.Count - 1];
        }

        public Card Pop()
        {
            Card card = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);
            return card;
        }

        public void Shuffle()
        {
            Random rng = new Random();
            deck = deck.OrderBy(_ => rng.Next()).ToList();
        }

        private void MakeDeck()
        {
            deck = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 1; i <= 13; i++)
                {
                    deck.Add(new Card(s, i, false));
                }
            }
        }
        public static CardDeck TwoDeck()
        {
            var deck = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 1; i <= 13; i++)
                {
                    deck.Add(new Card(s, i, false));
                    deck.Add(new Card(s, i, false));
                }
            }
            var BigDeck = new CardDeck { deck = deck };
            BigDeck.Shuffle();
            return BigDeck;

        }
        public (List<Card>, List<Card> cards) SplitDeck()
        {
            List<Card> Stock = deck.Take(50).ToList();
            List<Card> Spider = deck.Skip(50).Take(54).ToList();
            return (Stock, Spider);

        }
        //public override string ToString()
        //
        //    String s = "";
        //    foreach (Card c in deck)
        //    {
        //        s += c.Suit + "," + c.Value + "\n";
        //    }

        //    return s;
        //}

    }
}