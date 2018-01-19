using System.Collections.Generic;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Writing
{
    public interface INodeWeightInformer
    {
        double MinWidth { get; set; }
        double MaxWidth { get; set; }
        double GetWidth(Node n);
        void Init(IEnumerable<Node> nodes);
    }
}