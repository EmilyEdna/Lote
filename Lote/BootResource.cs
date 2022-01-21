using Lote.CommonWindow;
using Lote.Override;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote
{
    public static class BootResource
    {
        public static ConcurrentDictionary<string, NovelContentWindows> NovelContentWindow;
        static BootResource()
        {
            NovelContentWindow = new ConcurrentDictionary<string, NovelContentWindows>();
        }
    }
}
