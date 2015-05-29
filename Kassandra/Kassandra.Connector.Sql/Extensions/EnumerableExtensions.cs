using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kassandra.Connector.Sql.Extensions
{
    public static class EnumerableExtensions
    {
        public static DataTable ToIdTable(this IEnumerable<int> ids, string datatableColumnIdName)
        {
            DataTable tableIds = new DataTable();
            tableIds.Columns.Add(new DataColumn(datatableColumnIdName, typeof (int)));

            foreach (int id in ids.Distinct())
            {
                DataRow row = tableIds.NewRow();
                row[datatableColumnIdName] = id;
                tableIds.Rows.Add(row);
            }

            return tableIds;
        }

        public static DataTable ToUidTable(this IEnumerable<Guid> uids, string datatableColumnUidName)
        {
            DataTable tableIds = new DataTable();
            tableIds.Columns.Add(new DataColumn(datatableColumnUidName, typeof (int)));

            foreach (Guid id in uids.Distinct())
            {
                DataRow row = tableIds.NewRow();
                row[datatableColumnUidName] = id;
                tableIds.Rows.Add(row);
            }

            return tableIds;
        }
    }
}