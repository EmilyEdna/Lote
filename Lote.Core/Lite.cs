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
                ConnectionString = $"DataSource={DbRoute}",
                IsAutoCloseConnection = true,
            });
            return db;
        }

        private string DbRoute => SyncStatic.CreateFile(Path.Combine(SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDb")), "Lote.db"));

        public void InitDataBase()
        {
            Type[] Table = {
                typeof(LoteFavorite),
                typeof(LoteSetting),
                typeof(LotePlayList),
                typeof(LotePlayLyrics),
                typeof(LoteNovelHistory),
                typeof(LoteMangaHistory),
                typeof(LoteAnimeHistory)
            };
            LiteBase().CodeFirst.InitTables(Table);
        }
    }
}
