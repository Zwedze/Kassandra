using System;

namespace Kassandra.Core.Models.Query
{
    public class QueryErrorEventArgs
    {
        public Exception Exception { get; set; }
        public string AdditionalMessage { get; set; }
    }
}