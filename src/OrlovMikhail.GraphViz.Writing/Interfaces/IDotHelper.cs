namespace OrlovMikhail.GraphViz.Writing
{
    public interface IDotHelper
    {
        string EscapeId(string s);

        bool IsProperlyQuoted(string s);

        string GetRecordFromAttribute(IAttribute attribute);
    }
}