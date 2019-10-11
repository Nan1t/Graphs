namespace Graphs {

    public class GraphNodeInfo {

        public GraphNodeInfo(GraphNode node) {
            Node = node;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousNode = null;
        }

        public GraphNode Node { get; set; }

        public bool IsUnvisited { get; set; }

        public int EdgesWeightSum { get; set; }

        public GraphNode PreviousNode { get; set; }
    }
}
