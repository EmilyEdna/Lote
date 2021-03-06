using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Service.DTO
{
    public class LotePlayListDTO
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string CacheAddress { get; set; }
        public string SongName { get; set; }
        public string SongAlbum { get; set; }
        public string SongArtist { get; set; }
        public string SongId { get; set; }
        public long Span { get; set; }
        public int Platform { get; set; }
    }

    public class PagePlayListDTO {
        public int Count { get; set; }
        public int Total { get; set; }
        public List<LotePlayListDTO> Result { get; set; }
    }
}
