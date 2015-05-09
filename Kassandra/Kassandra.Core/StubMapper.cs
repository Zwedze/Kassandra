using System.Collections.Generic;
using Kassandra.Core.Interfaces;

namespace Kassandra.Core
{
    public class StubMapper<TOutput> : IMapper<TOutput>
    {
        public TOutput Map(IResultReader reader)
        {
            return default(TOutput);
        }

        public IList<TOutput> MapToList(IResultReader reader)
        {
            return default(IList<TOutput>);
        }
    }
}