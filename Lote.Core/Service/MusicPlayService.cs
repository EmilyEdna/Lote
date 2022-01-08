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
        List<PlayListDTO> GetPlayList();
        void AddPlayList(PlayListDTO input);
        void RemovePlayList(Guid Id);
    }
    public class MusicPlayService : Lite, IMusicPlayService
    {
        public List<PlayListDTO> GetPlayList()
        {
            return LiteBase().Queryable<PlayList>().OrderBy(t => t.Span, OrderByType.Desc).ToList().ToMapper<PlayList, PlayListDTO>();
        }

        public void AddPlayList(PlayListDTO input)
        {
            var play = input.ToMapest<PlayList>();
            LiteBase().Insertable(play).CallEntityMethod(t => t.Create()).ExecuteCommand();
        }


        public void RemovePlayList(Guid Id)
        {
            LiteBase().Deleteable<PlayList>(t => t.Id == Id).ExecuteCommand();
        }
    }
}
