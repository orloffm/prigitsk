using System;

namespace Prigitsk.Core.Nodes.Loading
{
    public sealed class RepositoryDataBuilderFactory : IRepositoryDataBuilderFactory
    {
        private readonly Func<IRepositoryDataBuilder> _builderMaker;

        public RepositoryDataBuilderFactory(Func<IRepositoryDataBuilder> builderMaker)
        {
            _builderMaker = builderMaker;
        }

        public IRepositoryDataBuilder CreateBuilder()
        {
            return _builderMaker();
        }
    }
}