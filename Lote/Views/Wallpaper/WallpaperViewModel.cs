using HandyControl.Data;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Wallpaper.SDK.ViewModel.Response;
using XExten.Advance.LinqFramework;

namespace Lote.Views.Wallpaper
{
    public class WallpaperViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly WallpaperProxy Proxy;
        public WallpaperViewModel(IContainer container)
        {
            this.PageIndex = 1;
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new WallpaperProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<WallpaperResult> _Wallpaper;
        public ObservableCollection<WallpaperResult> Wallpaper
        {
            get { return _Wallpaper; }
            set { SetAndNotify(ref _Wallpaper, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }
        #endregion

        #region Field
        private int Limit = 1;
        private string KeyWord;
        #endregion
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }

        protected override void OnViewLoaded()
        {
            var WallpaperInit = WallpaperFactory.Wallpaper(opt =>
            {
                opt.RequestParam = new WallpaperRequestInput
                {
                    CacheSpan = CacheTime(),
                    WallpaperType = WallpaperEnum.Init,
                    Init = new WallpaperInit
                    {
                        Limit = Limit
                    },
                    Proxy = this.Proxy
                };
            }).Runs();
            this.Total = (WallpaperInit.Total+Limit-1)/Limit;
            this.Wallpaper = new ObservableCollection<WallpaperResult>(WallpaperInit.Result);
        }

        public void Search(string args) 
        {
            KeyWord = args;
            var WallpaperSearch = WallpaperFactory.Wallpaper(opt =>
            {
                opt.RequestParam = new WallpaperRequestInput
                {
                    CacheSpan = CacheTime(),
                    WallpaperType = WallpaperEnum.Search,
                    Search = new WallpaperSearch
                    {
                        Limit = Limit,
                        KeyWord= KeyWord
                    },
                    Proxy = this.Proxy
                };
            }).Runs();
            this.Total = (WallpaperSearch.Total + Limit - 1) / Limit;
            this.Wallpaper = new ObservableCollection<WallpaperResult>(WallpaperSearch.Result);
        }

        public void PageUpdated(FunctionEventArgs<int> args) 
        {
            PageIndex = args.Info;
            if (KeyWord.IsNullOrEmpty())
            {
                var WallpaperInit = WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = CacheTime(),
                        WallpaperType = WallpaperEnum.Init,
                        Init = new WallpaperInit
                        {
                            Page = PageIndex,
                            Limit = Limit
                        },
                        Proxy = this.Proxy
                    };
                }).Runs();
                this.Total = (WallpaperInit.Total + Limit - 1) / Limit;
                this.Wallpaper = new ObservableCollection<WallpaperResult>(WallpaperInit.Result);
            }
            else
            {
                var WallpaperSearch = WallpaperFactory.Wallpaper(opt =>
                {
                    opt.RequestParam = new WallpaperRequestInput
                    {
                        CacheSpan = CacheTime(),
                        WallpaperType = WallpaperEnum.Search,
                        Search = new WallpaperSearch
                        {
                            Page = PageIndex,
                            Limit = Limit,
                            KeyWord = KeyWord
                        },
                        Proxy = this.Proxy
                    };
                }).Runs();
                this.Total = (WallpaperSearch.Total + Limit - 1) / Limit;
                this.Wallpaper = new ObservableCollection<WallpaperResult>(WallpaperSearch.Result);
            }
        }
    }
}
