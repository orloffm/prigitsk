﻿using System;
using System.Collections.Generic;
using Prigitsk.Core.Nodes;

namespace Prigitsk.Core.Graph.Writing
{
    [Obsolete]
    public interface INodeWeightInformer
    {
        double MaxWidth { get; set; }

        double MinWidth { get; set; }

        double GetWidth(INode n);

        void Init(IEnumerable<INode> nodes);
    }
}