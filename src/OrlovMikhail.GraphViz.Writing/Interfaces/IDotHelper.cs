namespace OrlovMikhail.GraphViz.Writing
{
    public interface IDotHelper
    {
        string EscapeId(string s);

        string GetRecordFromAttribute(IAttribute attribute);

        bool IsProperlyQuoted(string s);
    }
}