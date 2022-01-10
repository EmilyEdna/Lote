using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.CommonWindow.ViewMdeol
{
    public class MusicLyricWindowsViewModel : Screen
    {
        private readonly IContainer container;
        public MusicLyricWindowsViewModel(IContainer container)
        {
            this.container = container;
        }

        #region Property
        private string _Lyric;
        public string Lyric
        {
            get { return _Lyric; }
            set { SetAndNotify(ref _Lyric, value); }
        }
        #endregion
    }
}
