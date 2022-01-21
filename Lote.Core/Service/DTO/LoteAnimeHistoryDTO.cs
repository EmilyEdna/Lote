using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote.Core.Service.DTO
{
    public class LoteAnimeHistoryDTO
    {
        public Guid Id { get; set; }
        public long Span { get; set; }
        public string PlayURL { get; set; }
        public string AnimeName { get; set; }
        public string CollectName { get; set; }
        public bool PlayMode { get; set; }
        public string PlayModelDes => PlayMode ? "VLC" : "DPlayer";
    }
}
