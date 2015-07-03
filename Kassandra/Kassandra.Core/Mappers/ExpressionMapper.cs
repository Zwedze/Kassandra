using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Kassandra.Core.Components;

namespace Kassandra.Core.Mappers
{
    public class ExpressionMapper<TOutput> : IMapper<TOutput>
    {
        private readonly IList<MappingItem<TOutput>> _mappings;

        public ExpressionMapper()
        {
            _mappings = new List<MappingItem<TOutput>>();
        }

        public TOutput Map(IResultReader reader)
        {
            if (reader.Read())
            {
                return MapItem(reader);
            }

            return default(TOutput);
        }

        public IList<TOutput> MapToList(IResultReader reader)
        {
            List<TOutput> list = new List<TOutput>();
            while (reader.Read())
            {
                list.Add(MapItem(reader));
            }

            return list;
        }

        private TOutput MapItem(IResultReader reader)
        {
            TOutput output = Activator.CreateInstance<TOutput>();
            foreach (MappingItem<TOutput> mappingItem in _mappings)
            {
                Expression<Func<TOutput, object>> expression = mappingItem.Expression;
                LambdaExtensions.SetPropertyValue(output, expression, reader.ValueAs(expression.GetExpressionType(), mappingItem.ReaderKey));
            }

            return output;
        }

        public ExpressionMapper<TOutput> Bind(Expression<Func<TOutput, object>> expression, string readerKey)
        {
            _mappings.Add(new MappingItem<TOutput>(expression, readerKey));

            return this;
        }
    }
}