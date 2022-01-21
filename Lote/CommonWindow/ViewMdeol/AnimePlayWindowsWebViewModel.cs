using Anime.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.CommonWindow.ViewMdeol
{
    public class AnimePlayWindowsWebViewModel: Screen
    {
        private readonly IContainer container;
        public AnimePlayWindowsWebViewModel(IContainer container)
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
