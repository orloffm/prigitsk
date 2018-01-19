using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Writing
{
    public class NodeWeightInformer : INodeWeightInformer
    {
        private readonly double baseWidth = 0.2d;
        private int _maxChange;
        private double _maxWidth;
        private int _minChange;
        private double _minWidth;
        private double _widthDiff;

        public NodeWeightInformer()
        {
            MinWidth = 0.1d;
            MaxWidth = 0.6d;
        }

        public double MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value;
                _widthDiff = _maxWidth - _minWidth;
            }
        }

        public double MaxWidth
        {
            get => _maxWidth;
            set
            {
                _maxWidth = value;
                _widthDiff = _maxWidth - _minWidth;
            }
        }

        public void Init(IEnumerable<Node> nodes)
        {
            List<int> changes = nodes.Select(GetChange).ToList();
            if (changes.Count == 0)
            {
                _minChange = 0;
                _maxChange = 0;
                return;
            }

            changes.Sort();
            int minIndex;
            int maxIndex;
            if (changes.Count > 5)
            {
                minIndex = changes.Count / 4;
                maxIndex = minIndex * 3;
            }
            else
            {
                minIndex = 0;
                maxIndex = changes.Count - 1;
            }

            _minChange = changes[minIndex];
            _maxChange = changes[maxIndex];
        }

        public double GetWidth(Node n)
        {
            int diff = _maxChange - _minChange;
            if (diff == 0)
            {
                return baseWidth;
            }

            int change = GetChange(n);
            double result = MinWidth + _widthDiff * change / diff;
            result = Math.Max(result, MinWidth);
            result = Math.Min(result, MaxWidth);
            return result;
        }

        private int GetChange(Node n)
        {
            return n.Deletions + n.Insertions;
        }
    }
}