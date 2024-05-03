using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solitare
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public Stack<Card> Cards;

        public UserControl1(Stack<Card> cards)
        {
            InitializeComponent();
            Cards = cards;
        }

        public void SetCanvas(Canvas c)
        {
            Grid.SetRow(c, 0);
            Grid.SetColumn(c, 0);
            grid.Children.Add(c);
        }
    }
}