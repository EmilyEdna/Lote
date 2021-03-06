using LightNovel.SDK.ViewModel.Response;
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
    public class LightNovelContentWindowsViewModel : Screen
    {
        private readonly IContainer container;
        public LightNovelContentWindowsViewModel(IContainer container)
        {
            this.container = container;
            this.FontSize = 22;

        }

        #region Property
        private LightNovelContentResult _LightNovelContent;
        public LightNovelContentResult LightNovelContent
        {
            get { return _LightNovelContent; }
            set { SetAndNotify(ref _LightNovelContent, value); }
        }
        private bool _Show;
        public bool Show
        {
            get { return _Show; }
            set { SetAndNotify(ref _Show, value); }
        }
        private int _FontSize;
        public int FontSize
        {
            get { return _FontSize; }
            set { SetAndNotify(ref _FontSize, value); }
        }
        #endregion

        #region Method

        public ICommand SliderChange => new Commands<double>(args =>
        {
            FontSize = (int)args;
        }, null);
        #endregion
    }
}
