using System.Collections.Generic;

namespace Kassandra.Core
{
    public interface IMapper<TOutput>
    {
        TOutput Map(IResultReader reader);
        IList<TOutput> MapToList(IResultReader reader);
    }
}