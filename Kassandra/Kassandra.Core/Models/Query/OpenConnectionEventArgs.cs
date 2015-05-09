using Kassandra.Core.Interfaces;

namespace Kassandra.Core.Models.Query
{
    public class OpenConnectionEventArgs
    {
        public IConnection Connection { get; set; }
    }
}