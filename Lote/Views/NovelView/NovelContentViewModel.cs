using HandyControl.Data;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using SDKRequest = Novel.SDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lote.CommonWindow.ViewMdeol;
using Lote.CommonWindow;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using XExten.Advance.LinqFramework;

namespace Lote.Views.NovelView
{
    public class NovelContentViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IDictionary<string, NovelContentWindows> data;
        private readonly OptionRootDTO root;
        private readonly NovelProxy Proxy;
        public NovelContentViewModel(IContainer container)
        {
            this.container = container;
            this.data = new Dictionary<string, NovelContentWindows>();
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            Proxy = new NovelProxy
            {
                IP = root == null ? String.Empty:root.ProxyIP,
                PassWord = root == null ? String.Empty : root.ProxyPwd,
                Port = root == null ? -1 : Convert.ToInt32(root.ProxyPort),
                UserName = root == null ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private NovelDetailResult _NovelDetail;
        public NovelDetailResult NovelDetail
        {
            get { return _NovelDetail; }
            set { SetAndNotify(ref _NovelDetail, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }

        private string _Addr;
        public string Addr
        {
            get { return _Addr; }
            set { SetAndNotify(ref _Addr, value); }
        }
        #endregion

        #region Method
        public void PageUpdated(FunctionEventArgs<int> args)
        {
            PageIndex = args.Info;
            var NovelDetail = NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new NovelRequestInput
                {
                    CacheSpan = root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan),
                    NovelType = NovelEnum.Detail,
                    Proxy = this.Proxy,
                    Detail = new NovelDetail
                    {
                        Page = PageIndex,
                        NovelDetailAddress = Addr
                    }
                };
            }).Runs();
            this.NovelDetail = NovelDetail.Details;
        }

        public void ShowContent(string args)
        {
            if (string.IsNullOrEmpty(args))
                return;
            var NovelContent = NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new NovelRequestInput
                {
                    CacheSpan= root.CacheSpan.IsNullOrEmpty()?60:Convert.ToInt32(root.CacheSpan),
                    NovelType = NovelEnum.Watch,
                    Proxy = this.Proxy,
                    View = new SDKRequest.NovelView
                    {
                        NovelViewAddress = args
                    }
                };
            }).Runs();
            var vm = container.Get<NovelContentWindowsViewModel>();
            vm.NovelContent = NovelContent.Contents;
            NovelContentWindows win = null;
            if (data.ContainsKey(nameof(NovelContentWindows)))
            {
                win = data[nameof(NovelContentWindows)];
                win.DataContext = vm;
            }
            else
            {
                win = new NovelContentWindows();
                data[nameof(NovelContentWindows)] = win;
                win.DataContext = vm;
                win.Show();
            }

        }
        #endregion
    }
}
