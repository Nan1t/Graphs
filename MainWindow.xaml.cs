using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphs {
    public partial class MainWindow : Window {

        public static bool isLeftCTRL = false;
        private static MainWindow instance;

        public static MainWindow GetInstance()
        {
            return instance;
        }

        public MainWindow() {
            instance = this;
            InitializeComponent();

            Root.KeyDown += Grid_KeyDown;
            Root.KeyUp += Grid_KeyUp;
        }

        private void AddNode(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Right) {
                GraphNode node = new GraphNode();
                node.Index = Graph.LastIndex + 1;
                node.IndexLabel.Content = node.Index;

                Canvas.SetLeft(node, e.GetPosition(CanvasGrid).X - node.Width/2);
                Canvas.SetTop(node, e.GetPosition(CanvasGrid).Y - node.Height/2);
                CanvasGrid.Children.Add(node);

                Canvas.SetZIndex(node, 1);
                Graph.AddNode(node);
            }
        }

        private void ClearNodes(object sender, RoutedEventArgs e) {
            Graph.Clear();
        }

        private void ResetSelected(object sender, RoutedEventArgs e) {
            if (Graph.Selected1 != null) {
                Graph.Selected1.Border.Background = Brushes.Black;
                Graph.Selected1 = null;
            }
        }

        private void WirePoints(object sender, RoutedEventArgs e) {
            GraphEdge edge = Graph.WireSelected();

            if (edge != null) {
                CanvasGrid.Children.Add(edge.EdgeLine);
                Canvas.SetZIndex(edge.EdgeLine, 0);
            }
        }

        private void DFS(object sender, RoutedEventArgs e) {
            if (Graph.Selected1 != null) {
                Graph.ClearVisited();
                Graph.DFS(Graph.Selected1);
                return;
            }

            MessageBox.Show("You need to select some verticle");
        }

        private void FindSpanningTree(object sender, RoutedEventArgs e)
        {
            if (Graph.Selected1 != null)
            {
                Graph.ClearVisited();
                Graph.MakeSpanningTree(Graph.Selected1);
                return;
            }

            MessageBox.Show("You need to select some verticle");
        }

        private void BFS(object sender, RoutedEventArgs e) {
            if (Graph.Selected1 != null) {
                Graph.ClearVisited();
                Graph.BFS(Graph.Selected1);
                return;
            }
            MessageBox.Show("You need to select some verticle");
        }

        private void LoadGraph(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "graph files (*.graph)|*.graph|All files (*.*)|*.*";
            dialog.ShowDialog();

            if (dialog.CheckFileExists && dialog.FileName.Length > 0)
            {
                GraphLoader.Load(dialog.FileName);
            }
        }

        private void SaveGraph(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "graph files (*.graph)|*.graph|All files (*.*)|*.*";
            dialog.ShowDialog();

            if (dialog.FileName.Length > 0)
            {
                GraphLoader.Save(dialog.FileName);
            }
        }

        private void ClearPath(object sender, RoutedEventArgs e) {
            Graph.ClearVisited();
        }

        private void SearchMinPath(object sender, RoutedEventArgs e) {
            if (Graph.Selected1 != null && Graph.Selected2 != null) {
                List<GraphNode> list = Graph.ShowMinPath(Graph.Selected1, Graph.Selected2);

                if (list != null) {
                    Graph.ClearVisited();
                    GraphNode last = null;
                    foreach (GraphNode node in list) {
                        if (last != null) {
                            GraphEdge edge = Graph.GetEdgeBetween(last, node);

                            if (edge != null) {
                                edge.EdgeLine.Stroke = Brushes.Blue;
                            }
                        }
                        last = node;
                    }
                }

                return;
            }

            MessageBox.Show("You need to select 2 some verticles");
        }

        private void RandWeights(object sender, RoutedEventArgs e) {
            Graph.RandEdgesWeight();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                default:
                    break;
                case Key.Delete:
                    if (Graph.Selected1 != null) {
                        Graph.RemoveNode(Graph.Selected1);
                        CanvasGrid.Children.Remove(Graph.Selected1);
                        Graph.Selected1 = null;
                    }
                    break;
                case Key.LeftShift:
                case Key.LeftCtrl:
                    isLeftCTRL = true;
                    break;
                case Key.W:
                    WirePoints(null, null);
                    break;
            }
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e) {
            switch (e.Key) {
                default:
                    break;
                case Key.LeftShift:
                case Key.LeftCtrl:
                    isLeftCTRL = false;
                    break;
            }
        }
    }
}
