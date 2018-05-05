using System;
using System.Collections.Generic;
using Thinktecture.IO;

namespace OrlovMikhail.GraphViz.Writing
{
    public class GraphVizWriter : IGraphVizWriter
    {
        private const int TabIndent = 2;
        private readonly Stack<GraphHandle> _subGraphs;

        private readonly ITextWriter _w;

        public GraphVizWriter(ITextWriter w)
        {
            _w = w;
            _subGraphs = new Stack<GraphHandle>();
        }

        public void Comment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                EmptyLine();
                return;
            }

            bool isMultiline = comment.Contains("\n");
            if (isMultiline)
            {
                Line($"/* {comment} */");
            }
            else
            {
                Line($"// {comment}");
            }
        }

        public void Edge(string idA, string idB, IAttrSet attrSet = null)
        {
            throw new NotImplementedException();
        }

        public void EmptyLine()
        {
            _w.WriteLine();
        }

        public void EndGraph()
        {
            if (_subGraphs.Count != 1)
            {
                throw new InvalidOperationException();
            }

            EndGraphInternal();
        }


         void EndGraphInternal()
        {
          GraphHandle h= _subGraphs.Pop();
            h.MarkAsUsed();
            Line("}");
        }

        public void EndSubGraph()
        {
            if (_subGraphs.Count < 2)
            {
                throw new InvalidOperationException();
            }

            EndGraphInternal();
        }

        public void Node(string id, IAttrSet attributesSet = null)
        {
            throw new NotImplementedException();
        }

        public void RawAttributes(IAttrSet attributesSet = null)
        {
            throw new NotImplementedException();
        }

        public void SetEdgeAttributes(IAttrSet attributesSet = null)
        {
            throw new NotImplementedException();
        }

        public void SetGraphAttributes(IAttrSet attributesSet = null)
        {
            throw new NotImplementedException();
        }

        public void SetNodeAttributes(IAttrSet attributesSet = null)
        {
            throw new NotImplementedException();
        }

        public IGraphHandle StartGraph(GraphMode graphMode, bool strict)
        {
            if (_subGraphs.Count != 0)
            {
                throw new InvalidOperationException();
            }

            string line = "";
            if (strict)
            {
                line += "strict ";
            }

            line += graphMode == GraphMode.Graph ? "graph" : "digraph";
            line += " {";

            Line(line);

            GraphHandle g = new GraphHandle(this);
            _subGraphs.Push(g);
            return g;
        }

        public IGraphHandle StartSubGraph()
        {
            if (_subGraphs.Count == 0)
            {
                throw new InvalidOperationException();
            }

            Line("subgraph {");

            GraphHandle g = new GraphHandle(this);
            _subGraphs.Push(g);
            return g;
        }

        internal void EndGraphFromHandle(GraphHandle handle)
        {
            if (!ReferenceEquals(_subGraphs.Peek(), handle))
            {
                throw new InvalidOperationException();
            }

            EndGraphInternal();
        }

        private string GetCurrentIndent()
        {
            int count = _subGraphs.Count;
            return new string(' ', count * TabIndent);
        }

        private void Line(string text)
        {
            _w.Write(GetCurrentIndent());
            _w.WriteLine(text);
        }
    }

    public sealed class GraphHandle
        : IGraphHandle
    {
        private GraphVizWriter _w;

        public GraphHandle(GraphVizWriter w)
        {
            this._w = w;
        }

        public void Dispose()
        {
            if (_w != null)
            {
                _w.EndGraphFromHandle(this);
            }

            MarkAsUsed();
        }

        internal void MarkAsUsed()
        {
            _w = null;
        }
    }
}