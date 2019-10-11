using System.Collections.Generic;

namespace Graphs {
    public class Dijkstra {

        private List<GraphNodeInfo> infos;

        void InitInfo() {
            infos = new List<GraphNodeInfo>();
            foreach (GraphNode n in Graph.Nodes) {
                infos.Add(new GraphNodeInfo(n));
            }
        }

        GraphNodeInfo GetNodeInfo(GraphNode v) {
            foreach (var i in infos) {
                if (i.Node.Equals(v)) {
                    return i;
                }
            }

            return null;
        }

        public GraphNodeInfo FindUnvisitedNodeWithMinSum() {
            var minValue = int.MaxValue;
            GraphNodeInfo minNodeInfo = null;
            foreach (var i in infos) {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue) {
                    minNodeInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minNodeInfo;
        }

        public List<GraphNode> FindShortestPath(GraphNode startNode, GraphNode finishNode) {
            InitInfo();
            GraphNodeInfo first = GetNodeInfo(startNode);
            first.EdgesWeightSum = 0;
            while (true) {
                var current = FindUnvisitedNodeWithMinSum();
                if (current == null) {
                    break;
                }

                SetSumToNextNode(current);
            }

            return GetPath(startNode, finishNode);
        }

        void SetSumToNextNode(GraphNodeInfo info) {
            info.IsUnvisited = false;
            foreach (GraphEdge e in Graph.Edges[info.Node]) {
                GraphNode connectedNode = (info.Node == e.NodeFirst) ? e.NodeSecond : e.NodeFirst;
                var nextInfo = GetNodeInfo(connectedNode);
                var sum = info.EdgesWeightSum + e.Weigth;

                if (sum < nextInfo.EdgesWeightSum) {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousNode = info.Node;
                }
            }
        }

        List<GraphNode> GetPath(GraphNode startNode, GraphNode endNode) {
            List<GraphNode> list = new List<GraphNode>();

            list.Insert(0, endNode);

            while (startNode != endNode) {
                endNode = GetNodeInfo(endNode).PreviousNode;
                list.Insert(0, endNode);
            }

            return list;
        }
    }
}
