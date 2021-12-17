using LightNovel.SDK.ViewModel.Response;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.CommonWindow.ViewMdeol
{
    public class LightNovelContentWindowsViewModel : Screen
    {
        public LightNovelContentWindowsViewModel()
        {

        }

        #region Property
        private LightNovelContentResult _LightNovelContent;
        public LightNovelContentResult LightNovelContent
        {
            get { return _LightNovelContent; }
            set { SetAndNotify(ref _LightNovelContent, value); }
        }
        #endregion
    }
}
