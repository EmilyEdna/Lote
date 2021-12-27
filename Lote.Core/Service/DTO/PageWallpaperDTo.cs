using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Service.DTO
{
    public class PageWallpaperDTo
    {
        public int Total { get; set; }
        public List<WallpaperDTo> Result { get; set; }
    }
}
