using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Nodes.Loading
{
    public class NodeKeeperFactory : INodeKeeperFactory
    {
        private readonly ITimeHelper _timeHelper;
        private readonly ITreeManipulator _manipulator;

        public NodeKeeperFactory(ITimeHelper timeHelper, ITreeManipulator manipulator)
        {
            _timeHelper = timeHelper;
            _manipulator = manipulator;
        }

        public INodeKeeper CreateKeeper()
        {
            return new NodeKeeper(_timeHelper, _manipulator);
        }
    }
}