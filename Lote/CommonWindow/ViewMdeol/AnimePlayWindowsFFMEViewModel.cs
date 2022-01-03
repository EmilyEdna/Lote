using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.CommonWindow.ViewMdeol
{
    public class AnimePlayWindowsFFMEViewModel: Screen
    {
        private readonly IContainer container;
        public AnimePlayWindowsFFMEViewModel(IContainer container)
        {
            this.container = container;

        }

        #region Property
        private string _WatchRoute;
        public string WatchRoute
        {
            get { return _WatchRoute; }
            set { SetAndNotify(ref _WatchRoute, value); }
        }
        #endregion
    }
}
