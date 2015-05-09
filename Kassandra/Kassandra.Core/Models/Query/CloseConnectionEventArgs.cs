using Kassandra.Core.Interfaces;

namespace Kassandra.Core.Models.Query
{
    public class CloseConnectionEventArgs
    {
        public IConnection Connection { get; set; }
    }
}