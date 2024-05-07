using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Solitare
{
    public class Card
    {
        public Suit Suit { get; private set; }
        public int Value { get; private set; }
        public bool Uncovered { get; private set; }
        public Stack<Card> Stack { get; set; }
        public Image Image { get; set; }
        public Card(Suit suit, int value, bool uncovered)
        {
            this.Suit = suit;
            this.Value = value;
            Uncovered = uncovered;
        }
        public string GetCardImagePath()
        {
            if (Uncovered == false)
            {
                return $"/images/Hearts/Back.png";
            }

            string ValueToImage = Value.ToString();
            if (Value == 11)
            {
                ValueToImage = "J";
            }
            else if (Value == 12)
            {
                ValueToImage = "Q";
            }
            else if (Value == 13)
            {
                ValueToImage = "K";
            }
            else if (Value == 1)
            {
                ValueToImage = "A";
            }
            // Example: "Images/Hearts/Ace.png"
            return $"/images/Hearts/{Suit.EToString()}{ValueToImage}.png";

        }
        public void Uncover()
        {
            Uncovered = true;
        }
    }
}