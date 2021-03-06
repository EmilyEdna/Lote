using HandyControl.Data;
using Lote.CommonWindow;
using Lote.CommonWindow.ViewMdeol;
using Lote.Core.Common;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wallpaper.SDK;
using Wallpaper.SDK.ViewModel;
using Wallpaper.SDK.ViewModel.Enums;
using Wallpaper.SDK.ViewModel.Request;
using Wallpaper.SDK.ViewModel.Response;
using XExten.Advance.HttpFramework.MultiCommon;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Views.WallpaperViews
{
    public class WallpaperViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IWallpaperService wallpaperService;
        private readonly LoteSettingDTO root;
        private readonly WallpaperProxy Proxy;
        public WallpaperViewModel(IContainer container)
        {
            this.PageIndex = 1;
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new LoteSettingDTO();
            this.wallpaperService = container.Get<IWallpaperService>();
            this.Proxy = new WallpaperProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
            this.WatchFavorite = false;
        }

        #region Property
        private ObservableCollection<WallpaperResultDetail> _Wallpaper;
        public ObservableCollection<WallpaperResultDetail> Wallpaper
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

        private bool _WatchFavorite;
        public bool WatchFavorite
        {
            get { return _WatchFavorite; }
            set { SetAndNotify(ref _WatchFavorite, value); }
        }
        #endregion

        #region Field
        private int Limit = 12;
        private string KeyWord;
        #endregion

        #region Method
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }

        protected override void OnViewLoaded()
        {
            Task.Run(() => InitAll());
        }

        public void InitAll()
        {
            var favoriteId = wallpaperService.GetAllFavorite();
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
            if (favoriteId.Count > 0)
                WallpaperInit.GlobalResult.Result.ForEach(t =>
                {
                    if (favoriteId.Contains(t.Id))
                        t.IsFavorite = true;
                });

            this.Total = (WallpaperInit.GlobalResult.Total + Limit - 1) / Limit;
            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
        }

        public void InitFavorite(string args)
        {
            var data = wallpaperService.GetFavorite(args, PageIndex).Result;
            this.Total = data.Total;
            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(data.Result.ToMapest<List<WallpaperResultDetail>>());
        }

        public void Search(string args)
        {
            if (WatchFavorite)
            {
                InitFavorite(args);
            }
            else
            {
                var favoriteId = wallpaperService.GetAllFavorite();

                KeyWord = args;
                Task.Run(() =>
                {
                    var WallpaperSearch = WallpaperFactory.Wallpaper(opt =>
                    {
                        opt.RequestParam = new WallpaperRequestInput
                        {
                            CacheSpan = CacheTime(),
                            WallpaperType = WallpaperEnum.Search,
                            Search = new WallpaperSearch
                            {
                                Limit = Limit,
                                KeyWord = KeyWord
                            },
                            Proxy = this.Proxy
                        };
                    }).Runs();

                    if (favoriteId.Count > 0)
                        WallpaperSearch.GlobalResult.Result.ForEach(t =>
                        {
                            if (favoriteId.Contains(t.Id))
                                t.IsFavorite = true;
                        });
                    this.Total = (WallpaperSearch.GlobalResult.Total + Limit - 1) / Limit;
                    this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperSearch.GlobalResult.Result);
                });
            }
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            PageIndex = args.Info;
            if (WatchFavorite)
            {

                InitFavorite(KeyWord);
            }
            else
            {
                var favoriteId = wallpaperService.GetAllFavorite();
                if (KeyWord.IsNullOrEmpty())
                {
                    Task.Run(() =>
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
                        if (favoriteId.Count > 0)
                            WallpaperInit.GlobalResult.Result.ForEach(t =>
                            {
                                if (favoriteId.Contains(t.Id))
                                    t.IsFavorite = true;
                            });
                        this.Total = (WallpaperInit.GlobalResult.Total + Limit - 1) / Limit;
                        this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperInit.GlobalResult.Result);
                    });
                }
                else
                {
                    Task.Run(() =>
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
                        if (favoriteId.Count > 0)
                            WallpaperSearch.GlobalResult.Result.ForEach(t =>
                            {
                                if (favoriteId.Contains(t.Id))
                                    t.IsFavorite = true;
                            });
                        this.Total = (WallpaperSearch.GlobalResult.Total + Limit - 1) / Limit;
                        this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(WallpaperSearch.GlobalResult.Result);
                    });
                }
            }
        }

        public void Download(long Id)
        {
            var result = Wallpaper.FirstOrDefault(t => t.Id == Id);
            Task.Run(() =>
            {
                var bytes = IHttpMultiClient.HttpMulti
                     .InitWebProxy(this.Proxy.ToMapest<MultiProxy>())
                     .AddNode(t =>
                     {
                         t.NodePath = !result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg;
                         t.ReqType = MultiType.GET;
                     }).Build().RunBytes().FirstOrDefault();

                var FileName = (!result.OriginalPng.IsNullOrEmpty() ? result.OriginalPng : result.OriginalJepg).Split("/").LastOrDefault();

                var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteDown", "Wallpaper", $"{result.Author}"));
                var fn = SyncStatic.CreateFile(Path.Combine(dir, FileName));
                SyncStatic.WriteFile(bytes, fn);
                Process.Start("explorer.exe", dir);
            });
        }

        public void NoFavorite(long Id)
        {
            var dto = this.Wallpaper.FirstOrDefault(t => t.Id == Id).ToMapest<LoteWallpaperDTo>();
            wallpaperService.AddFavorite(dto);

            var Temp = this.Wallpaper.ToList();

            Temp.FirstOrDefault(t => t.Id == Id).IsFavorite = true;

            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(Temp);

        }

        public void Favorite(long Id)
        {
            wallpaperService.RemoveFavorite(Id);
            var Temp = this.Wallpaper.ToList();

            Temp.FirstOrDefault(t => t.Id == Id).IsFavorite = false;

            this.Wallpaper = new ObservableCollection<WallpaperResultDetail>(Temp);
        }

        public void Preview(long Id)
        {
            var vm = container.Get<WallpaperPreviewWindowsViewModel>();
            var wallpaper = Wallpaper.FirstOrDefault(t => t.Id == Id);
            vm.FileURL = wallpaper.OriginalPng.IsNullOrEmpty() ? wallpaper.FileSizeJepg : wallpaper.OriginalPng;

            BootResource.Wallpaper(window =>
            {
                window.DataContext = vm;
            });
        }
        #endregion
    }
}
