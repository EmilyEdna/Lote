using Lote.Core.Model;
using Lote.Core.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace Lote.Core.Service
{
    public interface IWallpaperService
    {
        Task RemoveFavorite(long Id);
        Task AddFavorite(LoteWallpaperDTo input);
        Task<PageWallpaperDTo> GetFavorite(string lable, int pageIndex);
        List<long> GetAllFavorite();
    }
    public class WallpaperService : Lite, IWallpaperService
    {
        public Task AddFavorite(LoteWallpaperDTo input)
        {
            var entity = input.ToMapest<LoteFavorite>();
            var incloud = LiteBase().Queryable<LoteFavorite>().Where(t => t.Id == input.Id).First();
            if (incloud != null)
                return Task.CompletedTask;
            return LiteBase().Insertable(entity).ExecuteCommandAsync();
        }

        public List<long> GetAllFavorite()
        {
            return LiteBase().Queryable<LoteFavorite>().Select(t => t.Id).ToList();
        }

        public Task<PageWallpaperDTo> GetFavorite(string lable, int pageIndex)
        {
            var Total = 0;
            var result = LiteBase().Queryable<LoteFavorite>()
                .WhereIF(!lable.IsNullOrEmpty(), t => t.Label.Contains(lable))
                .ToPageList(pageIndex, 12, ref Total);
            Total = (Total + 12 - 1) / 12;
            return Task.FromResult(new PageWallpaperDTo
            {
                Total = Total,
                Result = result.ToMapest<List<LoteWallpaperDTo>>()
            });
        }

        public Task RemoveFavorite(long Id)
        {
            return LiteBase().Deleteable<LoteFavorite>(t => t.Id == Id).ExecuteCommandAsync();
        }
    }
}
