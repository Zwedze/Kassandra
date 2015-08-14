using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kassandra.Core.Mappers
{
    public class FunctionMapper<TOutput> : IMapper<TOutput>
    {
        private readonly Func<IResultReader, TOutput> _mappingFunction;

        public FunctionMapper(Func<IResultReader, TOutput> mappingFunction)
        {
            _mappingFunction = mappingFunction;
        }

        public TOutput Map(IResultReader reader)
        {
            if (reader.Read())
            {
                return _mappingFunction.Invoke(reader);
            }
            return default(TOutput);
        }

        public IList<TOutput> MapToList(IResultReader reader)
        {
            IList<TOutput> list = new List<TOutput>();
            while (reader.Read())
            {
                list.Add(_mappingFunction(reader));
            }
            return list;
        }
    }
}