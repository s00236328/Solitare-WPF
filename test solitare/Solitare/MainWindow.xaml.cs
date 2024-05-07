using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Diagnostics;

namespace Solitare
{
    public partial class MainWindow : Window
    {
        private Table table = new Table();
        public MainWindow()
        {
            InitializeComponent();
            DisplayStacks();
            DisplayBottom();
        }
        private void DisplayStacks()
        {
            int stacks = 10;

            for (int i = 0; i < stacks; i++)
            {
                Canvas canvas = new Canvas();

                var currentStack = table.SpiderStacks[i].ToList();
                var reversed = new Stack<Card>(currentStack);
                UserControl1 userControl = new UserControl1(table.SpiderStacks[i]);
                userControl.AllowDrop = true;
                var stackIndex = i;
                DisplayTop(userControl, reversed ,canvas, stackIndex);
                Dropping(userControl, stackIndex, canvas);
               
            }
        }
        public void DisplayTop(UserControl1 userControl,Stack<Card> reversed,Canvas canvas,int stackIndex)
        {
            foreach (var card in reversed)
            {
                card.Stack = table.SpiderStacks[stackIndex];

                Image cardImage = new Image();
                card.Image = cardImage;
                cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
                cardImage.Width = 100;
                cardImage.Height = 80;

                AttachMouse(cardImage, card, stackIndex);

                canvas.Children.Add(cardImage);
                Canvas.SetTop(cardImage, 20 * canvas.Children.Count);
            }

            userControl.SetCanvas(canvas);
            Grid.SetColumn(userControl, stackIndex);
            Grid.SetRow(userControl, 0);
            TopStacksGrid.Children.Add(userControl);
        }
        public bool IsValidMove(Card targetCard, Stack<Card> targetStack)
        {
            if (targetStack.Count == 0)
            {
                // The stack is empty, any card can be placed here.
                return true;
            }
            else
            {
                var topCard = targetStack.Peek();
                // Check if the target card is one rank lower and of opposite color compared to the top card.
                return targetCard.Value == topCard.Value - 1 ;
            }
        }
        public void DisplayBottom()
        {
            // Display bottom stack
            StackPanel bottomStackPanel = new StackPanel();
            bottomStackPanel.Orientation = Orientation.Vertical;

            foreach (var card in table.Stock)
            {
                TextBlock cardText = new TextBlock();
                cardText.Text = card.GetCardImagePath();
                bottomStackPanel.Children.Add(cardText);
            }

            Grid.SetColumn(bottomStackPanel, 0);
            Grid.SetRow(bottomStackPanel, 0);
            BottomStackGrid.Children.Add(bottomStackPanel);
        }
        private Image CreateCardImage(Card card)
        {
            Image cardImage = new Image();
            card.Image = cardImage;
            cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
            cardImage.Width = 100;
            cardImage.Height = 80;
            return cardImage;
        }
        public void AttachMouse(Image cardImage,Card card,int stackIndex)
        {
            cardImage.MouseMove += (sender, e) =>
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    // Package the data.
                    DataObject data = new DataObject();

                    data.SetData("Object", card);
                    card.Stack = table.SpiderStacks[stackIndex];

                    // Initiate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            };          
        }

        public void Dropping(UserControl1 userControl, int stackIndex, Canvas canvas)
        {
            userControl.Drop += (sender, e) =>
            {
                base.OnDrop(e);
                if (e.Data.GetDataPresent("Object"))
                {
                    var draggedCard = (Card)e.Data.GetData("Object");
                    var targetStack = table.SpiderStacks[stackIndex];

                    if (IsValidMove(draggedCard, targetStack))
                    {
                        Image cardImage = CreateCardImage(draggedCard);
                        AttachMouse(cardImage, draggedCard, stackIndex);
                        canvas.Children.Remove(cardImage);
                        Canvas.SetTop(cardImage, 20 * canvas.Children.Count + 20);
                        canvas.Children.Add(cardImage);
                        draggedCard.Stack.Pop().Image.Source = null;
                        if (draggedCard.Stack.TryPeek(out var cardBelow))
                        {
                            cardBelow.Uncover();
                            cardBelow.Image.Source = new BitmapImage(new Uri(cardBelow.GetCardImagePath(), UriKind.Relative));
                        }
                        draggedCard.Stack = table.SpiderStacks[stackIndex];
                        draggedCard.Stack.Push(draggedCard);

                    }
                    else
                    {
                        MessageBox.Show("Invalid move!");
                    }
                }
            };
        }
    }
}


