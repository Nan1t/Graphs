using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Graphs {
    public partial class GraphNode : UserControl {

        public int Index { get; set; }
        public bool IsVisited { get; set; }
        public int Distance = int.MaxValue;
        private bool isClicked = false;

        public GraphNode() {
            InitializeComponent();

            this.MouseMove += OnMouseMove;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;
        }

        private void OnMouseMove(object sender, MouseEventArgs e) {
            if (isClicked) {
                double x = e.GetPosition((UIElement)this.Parent).X;
                double y = e.GetPosition((UIElement)this.Parent).Y;
                GraphNode node = sender as GraphNode;

                Canvas.SetLeft(node, x - node.Width / 2);
                Canvas.SetTop(node, y - node.Height / 2);
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                isClicked = true;

                if (MainWindow.isLeftCTRL) {
                    if (Graph.Selected1 == null || Graph.Selected2 == null) {
                        if (Graph.Selected1 == null) {
                            Graph.Selected1 = this;
                            Graph.Selected1.Border.Background = Brushes.Blue;
                            Graph.SelectIndex = false;
                            return;
                        }

                        if (Graph.Selected2 == null) {
                            Graph.Selected2 = this;
                            Graph.Selected2.Border.Background = Brushes.Blue;
                            Graph.SelectIndex = true;
                            return;
                        }
                    }

                    if (Graph.SelectIndex == false) {
                        // Last selected 1, select 2
                        Graph.Selected2.Border.Background = Brushes.Black;
                        Graph.Selected2 = this;
                        Graph.Selected2.Border.Background = Brushes.Blue;
                        Graph.SelectIndex = true;
                    }
                    else {
                        // Last selected 2, select 1
                        Graph.Selected1.Border.Background = Brushes.Black;
                        Graph.Selected1 = this;
                        Graph.Selected1.Border.Background = Brushes.Blue;
                        Graph.SelectIndex = false;
                    }

                    return;
                }

                if (Graph.Selected2 != null) {
                    Graph.Selected2.Border.Background = Brushes.Black;
                    Graph.Selected2 = null;
                }

                if (Graph.Selected1 != null) {
                    Graph.Selected1.Border.Background = Brushes.Black;
                }

                Graph.Selected1 = this;
                Graph.Selected1.Border.Background = Brushes.Blue;
                Graph.SelectIndex = false;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                isClicked = false;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs e) {
            Border.Background = Brushes.Blue;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e) {
            if (Graph.Selected1 != this && Graph.Selected2 != this) {
                Border.Background = Brushes.Black;
            }
            OnMouseMove(sender, e);
        }
    }
}
