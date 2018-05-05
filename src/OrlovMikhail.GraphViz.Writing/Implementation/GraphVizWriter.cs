using System;
using System.Collections.Generic;
using System.Text;
using Thinktecture.IO;

namespace OrlovMikhail.GraphViz.Writing
{
    public class GraphVizWriter : IGraphVizWriter
    {
        private const int TabIndent = 2;
        private readonly Stack<GraphHandle> _subGraphs;

        private readonly ITextWriter _w;
        private GraphMode _graphMode;
        private readonly IDotHelper _dotHelper;

        public GraphVizWriter(ITextWriter w, IDotHelper dotHelper)
        {
            _w = w;
            _subGraphs = new Stack<GraphHandle>();
            _dotHelper = dotHelper;
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

        public void Edge(string a, string b, IAttrSet attrSet = null)
        {
            string aw = _dotHelper.EscapeId(a);
            string bw = _dotHelper.EscapeId(b);

            StringBuilder sb = new StringBuilder();
            sb.Append(aw);
            sb.Append(' ');
            sb.Append(GetEdgeMarker());
            sb.Append(' ');
            sb.Append(bw);

            AppendAttributeSet(attrSet, sb);

            sb.Append(";");

            string wholeString = sb.ToString();
         Line(wholeString);
        }

        /// <summary>
        /// Serializes an attribute set to a string builder in form " [a = b]"
        /// </summary>
        private void AppendAttributeSet(IAttrSet attrSet, StringBuilder sb)
        {
            if (!AttrSet.NotNullOrEmpty(attrSet))
            {
                return;
            }

            sb.Append(" [");
            bool wroteFirst = false;
            foreach (var attribute in attrSet)
            {
                if (!wroteFirst)
                {
                    sb.Append(", ");
                }

                string record = _dotHelper.GetRecordFromAttribute(attribute);
                sb.Append(record);

                wroteFirst = true;
            }

            sb.Append("]");
        }


        private string GetEdgeMarker()
        {
            return _graphMode == GraphMode.Graph ? "--" : "->";
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

        public void EndSubGraph()
        {
            if (_subGraphs.Count < 2)
            {
                throw new InvalidOperationException();
            }

            EndGraphInternal();
        }

        public void Node(string id, IAttrSet attrSet = null)
        {
            string aw = _dotHelper.EscapeId(id);

            StringBuilder sb = new StringBuilder();
            sb.Append(aw);

            AppendAttributeSet(attrSet, sb);

            sb.Append(";");

            string wholeString = sb.ToString();
            Line(wholeString);
        }

        public void RawAttributes(IAttrSet attributesSet)
        {
            foreach (var attr in attributesSet)
            {
                string record = _dotHelper.GetRecordFromAttribute(attr);
                Line($"{record};");
            }
        }

        public void SetEdgeAttributes(IAttrSet attributesSet )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("edge");
            AppendAttributeSet(attributesSet, sb);
            Line(sb.ToString());
        }

        public void SetGraphAttributes(IAttrSet attributesSet )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("graph");
            AppendAttributeSet(attributesSet, sb);
            Line(sb.ToString());
        }

        public void SetNodeAttributes(IAttrSet attributesSet )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("node");
            AppendAttributeSet(attributesSet, sb);
            Line(sb.ToString());
        }

        public IGraphHandle StartGraph(GraphMode graphMode, bool strict)
        {
            this._graphMode = graphMode;
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

        /// <summary>
        /// Ends graph or subgraph when called by handle disposal.
        /// </summary>
        internal void EndGraphFromHandle(GraphHandle handle)
        {
            if (!ReferenceEquals(_subGraphs.Peek(), handle))
            {
                throw new InvalidOperationException();
            }

            EndGraphInternal();
        }

        /// <summary>
        /// Pops a graph handle from stack, no checks performed.
        /// </summary>
        private void EndGraphInternal()
        {
            GraphHandle h = _subGraphs.Pop();
            h.MarkAsUsed();
            Line("}");
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
}