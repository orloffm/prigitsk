using System;

namespace OrlovMikhail.GraphViz.Writing
{
    public interface IGraphVizWriter
    {
        void Comment(string comment);

        void EndGraph();

        IGraphHandle StartGraph(GraphMode graphMode, bool strict);

        void SetGraphAttributes(IAttrSet attributesSet = null);

        void SetNodeAttributes(IAttrSet attributesSet = null);

        void SetEdgeAttributes(IAttrSet attributesSet = null);

        /// <summary>
        /// Writes a node.
        /// </summary>
        /// <param name="id">The id. Automatically escaped.</param>
        /// <param name="attributesSet">Attributes to add to this node.</param>
        void Node(string id, IAttrSet attributesSet = null);

        /// <summary>
        /// Writes attributes.
        /// </summary>
        void RawAttributes(IAttrSet attributesSet = null);

        IGraphHandle StartSubGraph();

        void EndSubGraph();

        void Edge(string idA, string idB, IAttrSet attrSet = null);

        void EmptyLine();
    }
}