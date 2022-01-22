using Lote.Core.Model;
using Lote.Core.Service.DTO;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Core.Service
{
    public interface IHistoryService
    {
        Task AddNovelHistory(LoteNovelHistoryDTO input);
        List<LoteNovelHistoryDTO> GetNovelHistory();
        Task ClearNovelHistory();

        Task AddMangaHistory(LoteMangaHistoryDTO input);
        List<LoteMangaHistoryDTO> GetMangaHistory();
        Task ClearMangaHistory();

        Task AddAnimeHistory(LoteAnimeHistoryDTO input);
        List<LoteAnimeHistoryDTO> GetAnimeHistory();
        Task ClearAnimeHistory();
    }
    public class HistoryService : Lite, IHistoryService
    {
        #region 小说
        public async Task AddNovelHistory(LoteNovelHistoryDTO input)
        {
            var entity = input.ToMapest<LoteNovelHistory>();
            var Novel = LiteBase().Queryable<LoteNovelHistory>().Where(t => t.ChapterName == input.ChapterName && t.BookName == input.BookName).First();
            if (Novel == null)
                await LiteBase().Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public List<LoteNovelHistoryDTO> GetNovelHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return LiteBase().Queryable<LoteNovelHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(1, 100)
                  .ToMapest<List<LoteNovelHistoryDTO>>();
        }

        public async Task ClearNovelHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await LiteBase().Deleteable<LoteNovelHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion

        #region 漫画
        public async Task AddMangaHistory(LoteMangaHistoryDTO input)
        {
            var entity = input.ToMapest<LoteMangaHistory>();
            entity.Chapters= SyncStatic.Compress(entity.Chapters, SecurityType.Base64);
            var Novel = LiteBase().Queryable<LoteMangaHistory>().Where(t=>t.Name==input.Name&&t.Title==input.Title).First();
            if (Novel == null)
                await LiteBase().Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public List<LoteMangaHistoryDTO> GetMangaHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return LiteBase().Queryable<LoteMangaHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(1, 100)
                  .Select(t => new LoteMangaHistoryDTO
                  {
                      Address = t.Address,
                      Id = t.Id,
                      Name = t.Name,
                      Span = t.Span,
                      TagKey = t.TagKey,
                      Title = t.Title,
                      Chapters = SyncStatic.Decompress(t.Chapters, SecurityType.Base64)
                  }).ToList();
        }

        public async Task ClearMangaHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await LiteBase().Deleteable<LoteMangaHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion

        #region 动漫
        public async Task AddAnimeHistory(LoteAnimeHistoryDTO input)
        {
            var entity = input.ToMapest<LoteAnimeHistory>();
            var Novel = LiteBase().Queryable<LoteAnimeHistory>().Where(t => t.AnimeName == input.AnimeName && t.CollectName == input.CollectName).First();
            if (Novel == null)
                await LiteBase().Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
        }

        public List<LoteAnimeHistoryDTO> GetAnimeHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            return LiteBase().Queryable<LoteAnimeHistory>().Where(t => t.Span >= today)
                  .OrderBy(t => t.Span, OrderByType.Desc).ToPageList(1, 100)
                  .ToMapest<List<LoteAnimeHistoryDTO>>();

        }

        public async Task ClearAnimeHistory()
        {
            long today = DateTime.Now.ToFmtDate(-1, "yyyy-MM-dd").AsDateTime().Ticks;
            await LiteBase().Deleteable<LoteAnimeHistory>().Where(t => t.Span < today).ExecuteCommandAsync();
        }
        #endregion
    }
}
