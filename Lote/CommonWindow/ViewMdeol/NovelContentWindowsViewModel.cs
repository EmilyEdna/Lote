using HandyControl.Tools.Extension;
using Lote.Common;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
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
        private readonly OptionRootDTO root;
        private readonly NovelProxy Proxy;
        public NovelContentWindowsViewModel(IContainer container)
        {
            this.container = container;
            this.FontSize = 22;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new NovelProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
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

        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }

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
                    CacheSpan = CacheTime(),
                    NovelType = NovelEnum.Watch,
                    Proxy = this.Proxy,
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
