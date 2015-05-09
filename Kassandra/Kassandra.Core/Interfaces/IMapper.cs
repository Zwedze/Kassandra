using System.Collections.Generic;

namespace Kassandra.Core.Interfaces
{
    public interface IMapper<TOutput>
    {
        TOutput Map(IResultReader reader);
        IList<TOutput> MapToList(IResultReader reader);
    }
}