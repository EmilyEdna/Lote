using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Model
{
    [SugarTable("GlobalSetting")]
    public class GlobalSetting : BasicEntity
    {
        [SugarColumn(ColumnDataType ="varchar(80)", IsNullable = true)]
        public string ProxyIP { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable =true)]
        public string ProxyPort { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string ProxyAccount { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string ProxyPwd { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string CacheSpan { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string WkAccount { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string WkPwd { get; set; }
    }
}
