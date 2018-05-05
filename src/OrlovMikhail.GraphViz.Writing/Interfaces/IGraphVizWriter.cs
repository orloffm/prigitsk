namespace OrlovMikhail.GraphViz.Writing
{
    public interface IGraphVizWriter
    {
        void Comment(string comment);

        void Edge(string idA, string idB, IAttrSet attrSet = null);

        void EmptyLine();

        void EndGraph();

        void EndSubGraph();

        /// <summary>
        ///     Writes a node.
        /// </summary>
        /// <param name="id">The id. Automatically escaped.</param>
        /// <param name="attributesSet">Attributes to add to this node.</param>
        void Node(string id, IAttrSet attributesSet = null);

        /// <summary>
        ///     Writes attributes.
        /// </summary>
        void RawAttributes(IAttrSet attributesSet);

        void SetEdgeAttributes(IAttrSet attributesSet);

        void SetGraphAttributes(IAttrSet attributesSet);

        void SetNodeAttributes(IAttrSet attributesSet);

        IGraphHandle StartGraph(GraphMode graphMode, bool strict);

        IGraphHandle StartSubGraph();
    }
}