using System;

namespace Kassandra.Core
{
    public interface IConnection : IDisposable
    {
        bool KeepOpen { get; set; }
        string Name { get; set; }
    }
}