using System;
using System.Linq.Expressions;

namespace Kassandra.Core.Components
{
    public class MappingItem<TMappingOutput>
    {
        public MappingItem(Expression<Func<TMappingOutput, object>> expression, string readerKey)
        {
            Expression = expression;
            ReaderKey = readerKey;
        }

        public Expression<Func<TMappingOutput, object>> Expression { get; private set; }
        public string ReaderKey { get; private set; }
    }
}