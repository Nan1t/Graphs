using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphs {
    class Graph {

        public static List<GraphNode> Nodes { get; set; } = new List<GraphNode>();
        public static Dictionary<GraphNode, List<GraphEdge>> Edges { get; set; } = new Dictionary<GraphNode, List<GraphEdge>>();
        public static Dictionary<GraphNode, List<GraphNode>> Adjency { get; set; } = new Dictionary<GraphNode, List<GraphNode>>();
        public static GraphNode Selected1 { get; set; } = null;
        public static GraphNode Selected2 { get; set; } = null;
        public static bool SelectIndex { get; set; } = true; // false - Selected; true - Selected2
        public static int LastIndex { get; set; } = -1;

        public static GraphNode GetNode(int index)
        {
            foreach (GraphNode node in Nodes) {
                if (node.Index == index)
                {
                    return node;
                }
            }

            return null;
        }

        public static void DFS(GraphNode start) {
            Stack<GraphNode> stack = new Stack<GraphNode>();
            stack.Push(start);

            while (stack.Count > 0) {
                GraphNode current = stack.Pop();

                Console.WriteLine("Next->" + current.IndexLabel.Content);

                if (!current.IsVisited) {
                    current.IsVisited = true;
                }

                foreach (GraphNode neighbour in Adjency[current]) {
                    if (!neighbour.IsVisited) {
                        Console.WriteLine("Visit->" + current.IndexLabel.Content);

                        GraphEdge edge = GetEdgeBetween(current, neighbour);
                        edge.EdgeLine.Stroke = Brushes.Blue;
                        neighbour.IsVisited = true;
                        stack.Push(neighbour);
                    }
                }
            }
        }

        public static void BFS(GraphNode start) {
            Queue<GraphNode> queue = new Queue<GraphNode>();
            queue.Enqueue(start);

            while (queue.Count > 0) {
                GraphNode current = queue.Dequeue();

                Console.WriteLine("Next->" + current.IndexLabel.Content);

                if (!current.IsVisited) {
                    current.IsVisited = true;
                }

                foreach (GraphNode neighbour in Adjency[current]) {
                    if (!neighbour.IsVisited) {
                        Console.WriteLine("Visit->" + current.IndexLabel.Content);

                        GraphEdge edge = GetEdgeBetween(current, neighbour);
                        edge.EdgeLine.Stroke = Brushes.Blue;
                        neighbour.IsVisited = true;
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        public static void RandEdgesWeight() {
            Random rand = new Random();
            List<int> nums = new List<int>();

            foreach (List<GraphEdge> list in Edges.Values) {
                foreach (GraphEdge edge in list) {
                    if (!edge.IsVisited) {
                        edge.IsVisited = true;
                        int num = rand.Next(100);

                        while (nums.Contains(num)) {
                            num = rand.Next(100);
                        }

                        edge.IsVisited = true;
                        edge.Weigth = num;
                        nums.Add(num);

                        Label label = new Label();
                        label.Content = num;
                        label.FontWeight = FontWeights.Bold;
                        label.Foreground = Brushes.Red;

                        MultiBinding bindingX = new MultiBinding();
                        MultiBinding bindingY = new MultiBinding();

                        bindingX.Converter = new WeightPosConverter(edge.EdgeLine, true);
                        bindingY.Converter = new WeightPosConverter(edge.EdgeLine, false);

                        Binding bindPosX1 = new Binding { Source = edge.EdgeLine, Path = new PropertyPath(Line.X1Property) };
                        Binding bindPosX2 = new Binding { Source = edge.EdgeLine, Path = new PropertyPath(Line.X2Property) };
                        Binding bindPosY1 = new Binding { Source = edge.EdgeLine, Path = new PropertyPath(Line.Y1Property) };
                        Binding bindPosY2 = new Binding { Source = edge.EdgeLine, Path = new PropertyPath(Line.Y2Property) };

                        bindingX.Bindings.Add(bindPosX1);
                        bindingX.Bindings.Add(bindPosX2);
                        bindingY.Bindings.Add(bindPosY1);
                        bindingY.Bindings.Add(bindPosY2);

                        label.SetBinding(Canvas.LeftProperty, bindingX);
                        label.SetBinding(Canvas.TopProperty, bindingY);

                        edge.WeightLabel = label;

                        MainWindow.GetInstance().CanvasGrid.Children.Add(label);
                    }
                }
            }
        }

        public static List<GraphNode> ShowMinPath(GraphNode from, GraphNode to) {
            Dijkstra dijkstra = new Dijkstra();
            return dijkstra.FindShortestPath(from, to);
        }

        public static void MakeSpanningTree(GraphNode root) {
            Canvas canvas = MainWindow.GetInstance().CanvasGrid;
            Queue<GraphNode> queue = new Queue<GraphNode>();
            queue.Enqueue(root);

            double distTop = 10;
            double distLeft = canvas.ActualWidth / 2;

            while (queue.Count > 0) {
                GraphNode current = queue.Dequeue();

                if (!current.IsVisited) {
                    Canvas.SetTop(current, distTop);
                    Canvas.SetLeft(current, distLeft);
                    current.IsVisited = true;

                    Console.WriteLine("New level");
                }

                distTop += 64;

                foreach (GraphNode neighbour in Adjency[current]) {
                    if (!neighbour.IsVisited) {
                        neighbour.IsVisited = true;
                        queue.Enqueue(neighbour);

                        distLeft -= 64;

                        Canvas.SetTop(neighbour, distTop);
                        Canvas.SetLeft(neighbour, distLeft);

                        distLeft += 64*2;

                        List<GraphNode> toRemove = new List<GraphNode>();

                        foreach(GraphNode remainNode in Adjency[neighbour]) {
                            if (remainNode != current && remainNode.IsVisited) {
                                GraphEdge edge = GetEdgeBetween(remainNode, neighbour);

                                if (edge != null) {
                                    canvas.Children.Remove(edge.EdgeLine);
                                    canvas.Children.Remove(edge.WeightLabel);
                                    toRemove.Add(remainNode);
                                    Adjency[remainNode].Remove(neighbour);
                                    Edges[neighbour].Remove(edge);
                                    Edges[remainNode].Remove(edge);
                                }
                            }
                        }

                        foreach (GraphNode n in toRemove) {
                            Adjency[neighbour].Remove(n);
                        }
                    }
                }
            }
        }

        public static GraphEdge GetEdgeBetween(GraphNode node1, GraphNode node2) {
            foreach (GraphEdge edge in Edges[node1]) {
                if (edge.NodeFirst == node2 || edge.NodeSecond == node2) {
                    return edge;
                }
            }

            return null;
        }

        public static void ClearVisited() {
            foreach(GraphNode n in Nodes) {
                n.IsVisited = false;

                foreach (GraphEdge edge in Edges[n]) {
                    edge.EdgeLine.Stroke = Brushes.Black;
                }
            }
        }

        public static void Clear() {
            foreach (GraphNode node in Nodes) {
                Canvas canvas = node.Parent as Canvas;

                foreach (List<GraphEdge> list in Edges.Values) {
                    if (list != null) {
                        foreach (GraphEdge edge in list) {
                            if (edge != null) {
                                canvas.Children.Remove(edge.EdgeLine);
                                canvas.Children.Remove(edge.WeightLabel);
                            }
                        }
                    }
                }

                canvas.Children.Remove(node);
            }

            Selected1 = null;
            Selected2 = null;

            Edges.Clear();
            Nodes.Clear();
            Adjency.Clear();
            LastIndex = -1;
        }

        public static void AddNode(GraphNode node) {
            Nodes.Add(node);
            Edges.Add(node, new List<GraphEdge>());
            Adjency.Add(node, new List<GraphNode>());
            LastIndex++;
        }

        public static void RemoveNode(GraphNode node) {
            Canvas canvas = node.Parent as Canvas;

            if (canvas != null) {
                if (Edges.ContainsKey(node)) {
                    foreach (GraphEdge edge in Edges[node]) {
                        if(node == edge.NodeFirst) {
                            Edges[edge.NodeSecond].Remove(edge);
                        } else {
                            Edges[edge.NodeFirst].Remove(edge);
                        }

                        canvas.Children.Remove(edge.EdgeLine);
                        canvas.Children.Remove(edge.WeightLabel);
                    }

                    Edges.Remove(node);
                }

                canvas.Children.Remove(node);
                Nodes.Remove(node);
                Adjency.Remove(node);

                if (node.Index == LastIndex) {
                    LastIndex--;
                }
            }
        }

        public static GraphEdge WireSelected() {
            if (Selected1 != null && Selected2 != null) {
                return Wire(Selected1, Selected2);
            }

            return null;
        }

        public static GraphEdge Wire(GraphNode node1, GraphNode node2) {
            if (!IsWired(node1, node2))
            {
                Line line = new Line();
                line.StrokeThickness = 3;
                line.Stroke = Brushes.Black;

                Binding bindX1 = new Binding { Source = node1, Path = new PropertyPath(Canvas.LeftProperty) };
                Binding bindY1 = new Binding { Source = node1, Path = new PropertyPath(Canvas.TopProperty) };
                Binding bindX2 = new Binding { Source = node2, Path = new PropertyPath(Canvas.LeftProperty) };
                Binding bindY2 = new Binding { Source = node2, Path = new PropertyPath(Canvas.TopProperty) };

                bindX1.Converter = new PosConverter();
                bindY1.Converter = new PosConverter();
                bindX2.Converter = new PosConverter();
                bindY2.Converter = new PosConverter();

                line.SetBinding(Line.X1Property, bindX1);
                line.SetBinding(Line.Y1Property, bindY1);
                line.SetBinding(Line.X2Property, bindX2);
                line.SetBinding(Line.Y2Property, bindY2);

                GraphEdge edge = new GraphEdge();
                edge.NodeFirst = node1;
                edge.NodeSecond = node2;
                edge.EdgeLine = line;

                Edges[node1].Add(edge);
                Edges[node2].Add(edge);

                Adjency[node1].Add(node2);
                Adjency[node2].Add(node1);

                return edge;
            }

            return null;
        }

        private static bool IsWired(GraphNode node1, GraphNode node2) {
            if (Edges.ContainsKey(node1)) {
                foreach (GraphEdge edge in Edges[node1]) {
                    if (edge.NodeFirst == node2 || edge.NodeSecond == node2) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class GraphEdge {
        public Line EdgeLine { get; set; } = null;
        public GraphNode NodeFirst { get; set; } = null;
        public GraphNode NodeSecond { get; set; } = null;
        public Label WeightLabel { get; set; } = null;
        public int Weigth { get; set; } = 0;
        public bool IsVisited { get; set; } = false;
    }
}
