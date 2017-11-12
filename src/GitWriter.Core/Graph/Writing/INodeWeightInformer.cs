using System.Collections.Generic;
using GitWriter.Core.Nodes;

namespace GitWriter.Core.Graph.Writing
{
    public interface INodeWeightInformer
    {
        double MinWidth { get; set; }
        double MaxWidth { get; set; }
        double GetWidth(Node n);
        void Init(IEnumerable<Node> nodes);
    }
}