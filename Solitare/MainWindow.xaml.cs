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
//main
namespace Solitare
{
    public partial class MainWindow : Window
    {
        private Table table = new Table();
        private List<Canvas> canvases = new List<Canvas>();
        public MainWindow()
        {
            InitializeComponent();
            DisplayStacks();
            DisplayBottom();
        }
        private void DisplayStacks()
        {
            foreach (Canvas canvas in canvases)
            {
                canvas.Children.Clear();
            }
            canvases.Clear();
            int stacks = 10;

            for (int i = 0; i < stacks; i++)
            {
                Canvas canvas = new Canvas();
                canvases.Add(canvas);

                var currentStack = table.SpiderStacks[i].ToList();
                var reversed = new Stack<Card>(currentStack);
                CardUserControl userControl = new CardUserControl(table.SpiderStacks[i]);
                userControl.AllowDrop = true;
                var stackIndex = i;
                DisplayTop(userControl, reversed, canvas, stackIndex);
                Dropping(userControl, stackIndex, canvas);

            }

        }
        public void DisplayTop(CardUserControl userControl, Stack<Card> reversed, Canvas canvas, int stackIndex)
        {
          
            foreach (var card in reversed)
            {
                card.Stack = table.SpiderStacks[stackIndex];

                Image cardImage = new Image();
                card.Image = cardImage;
                cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
                cardImage.Width = 100;
                cardImage.Height = 80;

                AttachMouse(cardImage, card);

                canvas.Children.Add(cardImage);
                Canvas.SetTop(cardImage, 20 * canvas.Children.Count);
            }

            userControl.SetCanvas(canvas);
            Grid.SetColumn(userControl, stackIndex);
            Grid.SetRow(userControl, 0);
            TopStacksGrid.Children.Add(userControl);
        }
        public bool IsValidMove(Card targetCard, Stack<Card> targetStack, Card cardMoved)
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
                return cardMoved.Uncovered && targetCard.Value == topCard.Value - 1;
            }
        }
        public void DisplayBottom()
        {
            // Display bottom stack
            StackPanel bottomStackPanel = new StackPanel();
            bottomStackPanel.Orientation = Orientation.Horizontal;
            for (int i = 0; i < 5; i++)
            {
                var card = table.Stock.ElementAt(i); // Access card by index

                // Create an image for the card
                Image cardImage = new Image();
                cardImage.Width = 100;
                cardImage.Height = 80;
                cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));

                // Add click event handler to the image
                cardImage.MouseDown += (sender, e) =>
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        bottomStackPanel.Children.Remove(cardImage);
                        NewLayer(table.Stock); // Call NewLayer() with the card from stock
                    }
                };

                bottomStackPanel.Children.Add(cardImage);
            }

            Grid.SetColumn(bottomStackPanel, 0);
            Grid.SetRow(bottomStackPanel, 0);
            BottomStackGrid.Children.Add(bottomStackPanel);
        }
        public void NewLayer(List<Card> stockCards)
        {
            // Iterate through each stack and assign one card to each
            for (int i = 0; i < table.SpiderStacks.Count; i++)
            {
                // Check if there are still cards to distribute
                if (stockCards.Count > 0)
                {
                    // Remove the card from the stock and add it to the current stack
                    Card card = stockCards[0];
                    card.Uncover();
                   // table.SpiderStacks[i].Push(card);
                    UpdateCard(card, table.SpiderStacks[i]);
                    stockCards.RemoveAt(0); // Remove the card from the stock
                    
                }
                else
                {
                    // If there are no more cards to distribute, exit the loop
                    break;
                }
            }

            DisplayStacks(); // Update the UI after adding cards to stacks
        }

        public void AttachMouse(Image cardImage, Card card)
        {
            cardImage.MouseMove += (sender, e) =>
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    // Package the data.
                    DataObject data = new DataObject();

                    data.SetData("Object", card);
                    //card.Stack = table.SpiderStacks[stackIndex];
                   
                    // Initiate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            };
        }

        public void Dropping(CardUserControl userControl, int stackIndex, Canvas canvas)
        {
            userControl.Drop += (sender, e) =>
            {
                base.OnDrop(e);
                if (e.Data.GetDataPresent("Object"))
                {
         
                    var card = (Card)e.Data.GetData("Object");
                    var targetStack = table.SpiderStacks[stackIndex];

                    if (IsValidMove(card, targetStack, card))
                    {
                        Stack<Card> toMove = new Stack<Card>();
                        Card cardBelow;
                        bool cardReached = false;
                        while (!cardReached && card.Stack.TryPop(out cardBelow))
                        {
                            toMove.Push(cardBelow);
                            if (cardBelow.Equals(card))
                            {
                                cardReached = true;
                            }
                        }

                        if (card.Stack.TryPeek(out cardBelow))
                        {
                            cardBelow.Uncover();
                            cardBelow.Image.Source = new BitmapImage(new Uri(cardBelow.GetCardImagePath(), UriKind.Relative));
                        }

                        while (toMove.TryPop(out var cardToMove))
                        {
                            UpdateCard(cardToMove, targetStack);
                        }
                        DisplayStacks();
                        CheckForSequences();
                    }
                    else
                    {
                        MessageBox.Show("Invalid move!");
                    }
                }
            };
        }

        public void UpdateCard(Card card, Stack<Card> tagetStack)
        {
            Image cardImage = new Image();
            cardImage.Width = 100;
            cardImage.Height = 80;
            cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
            card.Image = cardImage;
            card.Stack = tagetStack;
            card.Stack.Push(card);
            AttachMouse(card.Image, card);
        }
      
        public void CheckForSequences()
        {
            foreach (var stack in table.SpiderStacks)
            {
                if (stack.Count >= 13)
                {
                    // Get the top card and its suit
                    var topCard = stack.Peek();
                    var suit = topCard.Suit;

                    // Check if all cards from K to A of the same suit exist in the stack
                    var sequenceExists = true;
                    for (int i = topCard.Value; i >= topCard.Value - 12; i--)
                    {
                        var cardInSequence = stack.FirstOrDefault(card => card.Value == i && card.Suit == suit);
                        if (cardInSequence == null)
                        {
                            sequenceExists = false;
                            break;
                        }
                    }

                    if (sequenceExists)
                    {
                        // Remove the cards from K to A of the same suit
                        for (int i = topCard.Value; i >= topCard.Value - 12; i--)
                        {
                            var cardToRemove = stack.First(card => card.Value == i && card.Suit == suit);
                            stack.Pop();
                        }
                    }
                }
            }
        }

    }

}



