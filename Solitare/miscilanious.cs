//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Diagnostics;
////this class is for code that i might need but i dont have anywhere to use it 

//namespace Solitare
//{
//    //StackPanel stackPanel = new StackPanel();
//    //stackPanel.Orientation = Orientation.Vertical;

//    //foreach (var card in table.SpiderStacks[i])
//    //{
//    //   Image cardImage = new Image();
//    //    Trace.WriteLine(card.GetCardImagePath());
//    //    cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(),UriKind.Relative));
//    //    stackPanel.Children.Add(cardImage);
//    //}

//    //Grid.SetColumn(stackPanel, i);
//    //Grid.SetRow(stackPanel, 0);
//    //TopStacksGrid.Children.Add(stackPanel);
//    internal class miscilanious
//    {

//        private void DisplayStacks()
//        {
//            int stacks = 10;
//            //double columnWidth = TopStacksGrid.ActualWidth / (double)stacks * 0.8;
//            // i hate wpf i hate wpf i hate wpf i hate wpf i hate wpf
//            // this stupid TopStacksGrid is not initialized yet and has a width of 0 how stupid is that who even thought of this as a feature what

//            // Display top stacks
//            for (int i = 0; i < stacks; i++)
//            {
//                Canvas canvas = new Canvas();

//                var currentStack = table.SpiderStacks[i].ToList();
//                var reversed = new Stack<Card>(currentStack);
//                UserControl1 userControl = new UserControl1(table.SpiderStacks[i]);
//                userControl.AllowDrop = true;
//                var stackIndex = i;
//                userControl.Drop += (sender, e) =>
//                {
//                    base.OnDrop(e);
//                    if (e.Data.GetDataPresent("Object"))
//                    {

//                        Image cardImage = new Image();
//                        cardImage.Width = 100;
//                        cardImage.Height = 80;
//                        var card = (Card)e.Data.GetData("Object");
//                        cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
//                        //MessageBox.Show(card.Value,card.Suit.ToString());
//                        Canvas.SetTop(cardImage, 20 * canvas.Children.Count + 20);
//                        canvas.Children.Add(cardImage);

//                        //card.Stack.Pop();
//                        card.Stack.Pop().Image.Source = null;
//                        if (card.Stack.TryPeek(out var cardBelow))
//                        {
//                            cardBelow.Uncover();
//                            cardBelow.Image.Source = new BitmapImage(new Uri(cardBelow.GetCardImagePath(), UriKind.Relative));
//                        }

//                        card.Stack = table.SpiderStacks[stackIndex];
//                        card.Stack.Push(card);
//                    }
//                };
//                // how???
//                foreach (var card in reversed)
//                {
//                    card.Stack = table.SpiderStacks[i];

//                    Image cardImage = new Image();
//                    card.Image = cardImage;
//                    cardImage.Source = new BitmapImage(new Uri(card.GetCardImagePath(), UriKind.Relative));
//                    cardImage.Width = 100;
//                    cardImage.Height = 80;
//                    if (card.Uncovered == true)
//                    {
//                        cardImage.MouseMove += (sender, e) =>
//                        {
//                            base.OnMouseMove(e);
//                            if (e.LeftButton == MouseButtonState.Pressed)
//                            {

//                                // Package the data.
//                                DataObject data = new DataObject();

//                                data.SetData("Object", card);
//                                card.Stack = table.SpiderStacks[stackIndex];

//                                // Initiate the drag-and-drop operation.
//                                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
//                            }
//                        };
//                    }
//                    canvas.Children.Add(cardImage);
//                    Canvas.SetTop(cardImage, 20 * canvas.Children.Count);
//                }

//                userControl.SetCanvas(canvas);
//                Grid.SetColumn(userControl, i);
//                Grid.SetRow(userControl, 0);
//                TopStacksGrid.Children.Add(userControl);

//            }

//            // Display bottom stack
//        }
//    }
//}
