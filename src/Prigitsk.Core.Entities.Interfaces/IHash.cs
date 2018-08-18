namespace Prigitsk.Core.Entities
{
    public interface IHash : ITreeish
    {
        string Value { get; }

        bool Equals(IHash other);

        string ToShortString();
    }
}