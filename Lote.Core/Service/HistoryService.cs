using Lote.Core.Model;
using Lote.Core.Service.DTO;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace Lote.Core.Service
{
    public interface IHistoryService
    {
        Task AddNovelHistory(LoteNovelHistoryDTO input);
        List<LoteNovelHistoryDTO> GetNovelHistory(int pageIndex);
        Task ClearNovelHistory();
    }
    public class HistoryService : Lite, IHistoryService
    {
        public async Task AddNovelHistory(LoteNovelHistoryDTO input)
        {
            var entity = input.ToMapest<LoteNovelHistory>();
            var Novel = LiteBase().Queryable<LoteNovelHistory>().Where(t => t.ChapterName == input.ChapterName && t.BookName == input.BookName).First();
            if (Novel == null)
                await LiteBase().Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public List<LoteNovelHistoryDTO> GetNovelHistory(int pageIndex)
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return LiteBase().Queryable<LoteNovelHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(pageIndex, 10)
                  .ToMapest<List<LoteNovelHistoryDTO>>();
        }

        public async Task ClearNovelHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await LiteBase().Deleteable<LoteNovelHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
    }
}
