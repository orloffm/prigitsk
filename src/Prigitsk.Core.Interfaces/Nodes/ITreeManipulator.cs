namespace Prigitsk.Core.Nodes
{
    public interface ITreeManipulator
    {
        void AddChild(INode source, INode immediateChild);

        void AddParent(INode source, INode immediateParent);

        void RemoveItselfFromTheNodeGraph(INode n);
    }
}