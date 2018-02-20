using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Writing
{
    public class NodeWeightInformer : INodeWeightInformer
    {
        private readonly double _baseWidth = 0.2d;
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

        public double MaxWidth
        {
            get => _maxWidth;
            set
            {
                _maxWidth = value;
                _widthDiff = _maxWidth - _minWidth;
            }
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

        private int GetChange(INode n)
        {
            return n.Deletions + n.Insertions;
        }

        public double GetWidth(INode n)
        {
            int diff = _maxChange - _minChange;
            if (diff == 0)
            {
                return _baseWidth;
            }

            int change = GetChange(n);
            double result = MinWidth + _widthDiff * change / diff;
            result = Math.Max(result, MinWidth);
            result = Math.Min(result, MaxWidth);
            return result;
        }

        public void Init(IEnumerable<INode> nodes)
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
    }
}