using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tests.StubEntities
{
    public sealed class SignatureStub : ISignature
    {
        public string EMail { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? When { get; set; }
    }
}
