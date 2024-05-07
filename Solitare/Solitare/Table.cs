using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Solitare
{
    class Table
    {
        public List<Stack<Card>> SpiderStacks = new List<Stack<Card>>(10);
        public List<Card> Stock;

        public Table()
        {
            for (int i = 0; i < 10; i++)
            {
                SpiderStacks.Add(new Stack<Card>());
            }
            var (stock, spider) = CardDeck.TwoDeck().SplitDeck();
            Stock = stock;
            int counter = 0;

            foreach (Card card in spider)
            {
                SpiderStacks[counter].Push(card);
                if (counter == 9)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }
            }
            foreach (var stack in SpiderStacks)
            {
                var topcard = stack.Pop();
                topcard.Uncover();
                stack.Push(topcard);

            }
        }
    }
}