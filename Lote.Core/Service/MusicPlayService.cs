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
        List<PlayLyricsDTO> GetLyrics(string SongId, int Platform);
        void AddLyric(string SongId, int Platform, string Lyric);
        string GetPlayRoute(Guid Id);
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

        public string GetPlayRoute(Guid Id)
        {
            return LiteBase().Queryable<PlayList>().Where(t => t.Id == Id).First().CacheAddress;
        }

        public List<PlayLyricsDTO> GetLyrics(string SongId, int Platform)
        {
            var result = LiteBase().Queryable<PlayLyrics>()
                 .Where(t => t.SongId == SongId)
                 .Where(t => t.Platform == Platform)
                 .First();
            if (result == null)
                return null;
            else
            {
                List<PlayLyricsDTO> dto = new List<PlayLyricsDTO>();
                result.Lyric.Split("_").ForArrayEach<string>(item =>
                {
                    var data = item.Split("|");
                    dto.Add(new PlayLyricsDTO
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
            var model = new PlayLyrics
            {
                Lyric = Lyric,
                Platform = Platform,
                SongId = SongId,
            };
            LiteBase().Insertable(model).CallEntityMethod(t => t.Create()).ExecuteCommand();
        }
    }
}
