using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Graphs {
    class GraphLoader {

        /*
         * Format
         * x,y,index; x,y,index; x,y,index; x,y,index - Vertexes
         * index|index,index,index; index|index,index - Adjency
         */

        public static void Load(string file)
        {
            Graph.Clear();

            StreamReader reader = new StreamReader(@file, true);
            string line = null;
            int index = 0;

            while ((line = reader.ReadLine()) != null)
            {
                if (index == 0) // Vertexes data
                {
                    string[] arr = line.Split(';');

                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] data = arr[i].Split('.');

                        if (data.Length < 3) continue;

                        GraphNode node = new GraphNode();

                        node.Index = int.Parse(data[2]);
                        node.IndexLabel.Content = node.Index;

                        Canvas.SetLeft(node, double.Parse(data[0]));
                        Canvas.SetTop(node, double.Parse(data[1]));
                        MainWindow.GetInstance().CanvasGrid.Children.Add(node);

                        Canvas.SetZIndex(node, 1);
                        Graph.AddNode(node);
                    }
                }

                if (index == 1) // Adjency data
                {
                    string[] arr = line.Split(';');
                    for(int i = 0; i < arr.Length; i++)
                    {
                        string[] keyData = arr[i].Split('|');

                        if (keyData.Length < 2) continue;

                        int key = int.Parse(keyData[0]);
                        GraphNode node = Graph.GetNode(key);

                        if (node != null)
                        {
                            string[] neighbours = keyData[1].Split(',');

                            for (int j = 0; j < neighbours.Length-1; j++)
                            {
                                int nKey = int.Parse(neighbours[j]);
                                GraphNode n = Graph.GetNode(nKey);

                                if (n != null)
                                {
                                    GraphEdge edge = Graph.Wire(node, n);

                                    if (edge != null)
                                    {
                                        MainWindow.GetInstance().CanvasGrid.Children.Add(edge.EdgeLine);
                                        Canvas.SetZIndex(edge.EdgeLine, 0);
                                    }
                                }
                            }
                        }
                    }
                }

                index++;
            }
        }

        public static void Save(string file)
        {
            StreamWriter writer = new StreamWriter(@file, true);
            string line = null;

            foreach (GraphNode node in Graph.Nodes)
            {
                line += Canvas.GetLeft((UIElement)node) + "." + Canvas.GetTop((UIElement)node) + "." + node.IndexLabel.Content + ";";
            }

            writer.WriteLine(line);
            line = "";

            foreach (GraphNode node in Graph.Nodes)
            {
                line += node.IndexLabel.Content + "|";
                List<GraphNode> adj = Graph.Adjency[node];

                foreach (GraphNode neighbour in adj)
                {
                    line += neighbour.IndexLabel.Content + ",";
                }

                line += ";";
            }

            writer.WriteLine(line);
            writer.Close();
        }
    }
}
