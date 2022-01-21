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
        List<LotePlayListDTO> GetPlayList();
        void AddPlayList(LotePlayListDTO input);
        void RemovePlayList(Guid Id);
        List<LotePlayLyricsDTO> GetLyrics(string SongId, int Platform);
        void AddLyric(string SongId, int Platform, string Lyric);
        string GetPlayRoute(Guid Id);
    }
    public class MusicPlayService : Lite, IMusicPlayService
    {
        public List<LotePlayListDTO> GetPlayList()
        {
            return LiteBase().Queryable<LotePlayList>().OrderBy(t => t.Span, OrderByType.Desc).ToList().ToMapper<LotePlayList, LotePlayListDTO>();
        }

        public void AddPlayList(LotePlayListDTO input)
        {
            var play = input.ToMapest<LotePlayList>();
            LiteBase().Insertable(play).CallEntityMethod(t => t.Create()).ExecuteCommand();
        }

        public void RemovePlayList(Guid Id)
        {
            LiteBase().Deleteable<LotePlayList>(t => t.Id == Id).ExecuteCommand();
        }

        public string GetPlayRoute(Guid Id)
        {
            return LiteBase().Queryable<LotePlayList>().Where(t => t.Id == Id).First().CacheAddress;
        }

        public List<LotePlayLyricsDTO> GetLyrics(string SongId, int Platform)
        {
            var result = LiteBase().Queryable<LotePlayLyrics>()
                 .Where(t => t.SongId == SongId)
                 .Where(t => t.Platform == Platform)
                 .First();
            if (result == null)
                return null;
            else
            {
                List<LotePlayLyricsDTO> dto = new List<LotePlayLyricsDTO>();
                result.Lyric.Split("_").ForArrayEach<string>(item =>
                {
                    var data = item.Split("|");
                    dto.Add(new LotePlayLyricsDTO
                    {
                        Lyric = data.LastOrDefault(),
                        Time = data.FirstOrDefault()
                    });
                });
                return dto;
            }
        }

        public void AddLyric(string SongId, int Platform, string Lyric)
        {
            var model = new LotePlayLyrics
            {
                Lyric = Lyric,
                Platform = Platform,
                SongId = SongId,
            };
            LiteBase().Insertable(model).CallEntityMethod(t => t.Create()).ExecuteCommand();
        }
    }
}
