﻿namespace OrlovMikhail.GraphViz.Writing
{
    public class ColorAttribute : GraphVizColorAttribute
    {
        public ColorAttribute(IGraphVizColor value) : base(value)
        {
        }

        public override string Key => "color";
    }
}