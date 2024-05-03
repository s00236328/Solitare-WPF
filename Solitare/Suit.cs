using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare
{
    public enum Suit
    {
        C,
        H,
        D,
        S,
    }
    public static class SuitExtension
    {
        public static string EToString(this Suit suit)
        {
            return suit switch
            {
                Suit.D => "D",
                Suit.C => "C",
                Suit.H => "H",
                Suit.S => "S"

            };
        }
    }

}