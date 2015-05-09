using Kassandra.Core.Interfaces;

namespace Kassandra.Core.Models.Query
{
    public class QueryExecutionEventArgs
    {
        public IQuery Query { get; set; }
    }
}