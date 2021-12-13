using Lote.Common;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Lote.CommonWindow.ViewMdeol
{
    public class NovelContentWindowsViewModel : Screen
    {
        private readonly IContainer container;
        public NovelContentWindowsViewModel(IContainer container)
        {
            this.container = container;
            this.FontSize = 22;
        }
        #region Property
        private NovelContentResult _NovelContent;
        public NovelContentResult NovelContent
        {
            get { return _NovelContent; }
            set { SetAndNotify(ref _NovelContent, value); }
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

        public ICommand ShowContent => new Commands<string>(args =>
        {
            if (string.IsNullOrEmpty(args))
                return;
            var NovelContent = NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new NovelRequestInput
                {
                    NovelType = NovelEnum.Watch,
                    Proxy = new NovelProxy(),
                    View = new NovelView
                    {
                        NovelViewAddress = args
                    }
                };
            }).Runs();
            this.NovelContent = NovelContent.Contents;
        }, null);
        #endregion
    }
}
