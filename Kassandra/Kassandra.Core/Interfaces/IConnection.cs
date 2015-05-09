using System;

namespace Kassandra.Core.Interfaces
{
    public interface IConnection : IDisposable
    {
        bool KeepOpen { get; set; }
        string Name { get; set; }
    }
}