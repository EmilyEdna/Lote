using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Model
{
    public class BasicEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [SugarColumn(ColumnDataType = "bigint", IsNullable = true)]
        public long Span { get; set; }
        public void Create()
        {
            this.Id = Guid.NewGuid();
            this.Span = DateTime.Now.Ticks;
        }
    }
}
