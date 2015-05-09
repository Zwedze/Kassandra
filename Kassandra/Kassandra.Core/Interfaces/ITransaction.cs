using System;

namespace Kassandra.Core.Interfaces
{
    public interface ITransaction : IDisposable, IQueryBuilder
    {
        void AddQuery(IQuery query);
        void Commit();

        void Rollback();
    }
}