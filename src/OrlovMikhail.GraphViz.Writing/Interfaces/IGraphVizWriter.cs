namespace OrlovMikhail.GraphViz.Writing
{
    public interface IGraphVizWriter
    {
        void Comment(string comment);

        void EndGraph();

        IGraphHandle StartGraph(GraphMode graphMode, bool strict);

        void WriteGraphAttributes(IAttrSet attributesSet);

        void WriteNodeAttributes(IAttrSet attributesSet);

        /// <summary>
        /// Writes a node.
        /// </summary>
        /// <param name="id">The id. Automatically escaped.</param>
        /// <param name="attributesSet">Attributes to add to this node.</param>
        void Node(string id, IAttrSet attributesSet);
    }
}