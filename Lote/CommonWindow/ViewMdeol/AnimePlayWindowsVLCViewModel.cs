using Anime.SDK.ViewModel.Response;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using Lote.Common;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lote.CommonWindow.ViewMdeol
{
    public class AnimePlayWindowsVLCViewModel : Screen
    {
        private readonly IContainer container;
        public AnimePlayWindowsVLCViewModel(IContainer container)
        {
            this.container = container;
           
        }

        #region Property
        private AnimePlayResult _WatchResult;
        public AnimePlayResult WatchResult
        {
            get { return _WatchResult; }
            set { SetAndNotify(ref _WatchResult, value); }
        }
        #endregion
    }
}
