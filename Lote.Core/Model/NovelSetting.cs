using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Model
{
    [SugarTable("NovelSetting")]
    public class NovelSetting:BasicEntity
    {
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = false)]
        public string Account { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)", IsNullable = false)]
        public string Password { get; set; }
    }
}
