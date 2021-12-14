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
        public string? IP { get; set; }
        [SugarColumn(ColumnDataType = "int",IsNullable =true)]
        public int? Port { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string? UserName { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = true)]
        public string? PassWord { get; set; }
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? CacheSpan { get; set; }
    }
}
