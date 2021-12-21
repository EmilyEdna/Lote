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
    public class AnimePlayWindowsViewModel : Screen
    {
        private readonly IContainer container;
        public AnimePlayWindowsViewModel(IContainer container)
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
