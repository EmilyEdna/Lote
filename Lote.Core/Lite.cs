using Lote.Core.Common;
using Lote.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;

namespace Lote.Core
{
    public class Lite
    {
        public SqlSugarScope LiteBase()
        {
            SqlSugarScope db = new SqlSugarScope(new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                ConnectionString = $"DataSource={FileUtily.Instance.CreateFile("LoteDb", "Lote.db")}",
                IsAutoCloseConnection = true,
            });
            return db;
        }
        public void InitDataBase()
        {
            var Table = SyncStatic.Assembly("Lote.Core").SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == typeof(BasicEntity))).ToList();
            Table.Add(typeof(Favorite));
            LiteBase().CodeFirst.InitTables(Table.ToArray());
        }
    }
}
