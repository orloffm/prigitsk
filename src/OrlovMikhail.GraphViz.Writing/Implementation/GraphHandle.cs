namespace OrlovMikhail.GraphViz.Writing
{
    public sealed class GraphHandle
        : IGraphHandle
    {
        private GraphVizWriter _w;

        public GraphHandle(GraphVizWriter w)
        {
            _w = w;
        }

        public void Dispose()
        {
            _w?.EndGraphFromHandle(this);

            MarkAsUsed();
        }

        internal void MarkAsUsed()
        {
            _w = null;
        }
    }
}