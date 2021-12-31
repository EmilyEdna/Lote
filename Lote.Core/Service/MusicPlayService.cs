using Lote.Core.Model;
using Lote.Core.Service.DTO;
using XExten.Advance.LinqFramework;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Service
{
    public interface IMusicPlayService
    {
        PagePlayListDTO GetPlayList(int PageIndex = 1);
        void AddPlayList(PlayListDTO input);
    }
    public class MusicPlayService : Lite, IMusicPlayService
    {
        public PagePlayListDTO GetPlayList(int PageIndex = 1)
        {
            var Total = 0;
            var query = LiteBase().Queryable<PlayList>();
            var Count = query.Count();
            var Result = query.ToPageList(PageIndex, 15, ref Total).ToMapper<PlayList, PlayListDTO>();
            return new PagePlayListDTO
            {
                Count = Count,
                Total = Total,
                Result = Result
            };
        }

        public void AddPlayList(PlayListDTO input)
        {
            var play = input.ToMapest<PlayList>();
            LiteBase().Insertable(play).CallEntityMethod(t => t.Create()).ExecuteCommand();
        }
    }
}
