using System;

namespace Kassandra.Core
{
    public interface ITransaction : IDisposable, IQueryBuilder
    {
        void AddQuery(IQuery query);
        void Commit();

        void Rollback();
    }
}