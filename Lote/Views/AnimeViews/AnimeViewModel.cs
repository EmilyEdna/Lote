using Anime.SDK;
using Anime.SDK.ViewModel;
using Anime.SDK.ViewModel.Enums;
using Anime.SDK.ViewModel.Request;
using Anime.SDK.ViewModel.Response;
using HandyControl.Data;
using Lote.CommonWindow;
using Lote.CommonWindow.ViewMdeol;
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
using XExten.Advance.LinqFramework;

namespace Lote.Views.AnimeViews
{
    public class AnimeViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly AnimeProxy Proxy;
        private readonly IDictionary<string, AnimePlayWindowsByVLC> VLC;
        private readonly IDictionary<string, AnimePlayWindowsByWEB> DPlayer;
        public AnimeViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new AnimeProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
            LetterCate = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(",").ToList();
            VLC = new Dictionary<string, AnimePlayWindowsByVLC>();
            DPlayer = new Dictionary<string, AnimePlayWindowsByWEB>();
            PageIndex = 1;
        }

        #region Property
        private List<string> _LetterCate;
        public List<string> LetterCate
        {
            get { return _LetterCate; }
            set { SetAndNotify(ref _LetterCate, value); }
        }

        private Dictionary<string, string> _RecommendCategory;
        public Dictionary<string, string> RecommendCategory
        {
            get { return _RecommendCategory; }
            set { SetAndNotify(ref _RecommendCategory, value); }
        }

        private ObservableCollection<AnimeWeekDayResult> _WeekDay;
        public ObservableCollection<AnimeWeekDayResult> WeekDay
        {
            get { return _WeekDay; }
            set { SetAndNotify(ref _WeekDay, value); }
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

        private ObservableCollection<AnimeSearchResults> _Result;
        public ObservableCollection<AnimeSearchResults> Result
        {
            get { return _Result; }
            set { SetAndNotify(ref _Result, value); }
        }

        private ObservableCollection<AnimeDetailResult> _Detail;
        public ObservableCollection<AnimeDetailResult> Detail
        {
            get { return _Detail; }
            set { SetAndNotify(ref _Detail, value); }
        }

        #endregion

        #region Field
        private string SearchKey;
        private string CategoryKey;
        #endregion

        #region Method
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }

        protected override void OnViewLoaded()
        {
            Task.Run(() =>
            {
                var AnimeInit = AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Init,
                        Proxy = this.Proxy,
                    };
                }).Runs();
                this.RecommendCategory = AnimeInit.RecommendCategory;
                this.WeekDay = new ObservableCollection<AnimeWeekDayResult>(AnimeInit.WeekDays);
            });
        }

        public void SearchAnime(string args)
        {
            SearchKey = args;
            CategoryKey = string.Empty;
            Task.Run(() =>
            {
                var AnimeSearch = AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.Search,
                        CacheSpan = CacheTime(),
                        Proxy = this.Proxy,
                        Search = new AnimeSearch
                        {
                            AnimeSearchKeyWord = SearchKey,
                            Page = PageIndex
                        }
                    };
                }).Runs();
                this.Total = AnimeSearch.SeachResults.Page;
                this.Result = new ObservableCollection<AnimeSearchResults>(AnimeSearch.SeachResults.Searchs);
            });
        }

        public void Redirect(string args)
        {
            Task.Run(() =>
            {
                var AnimeDetail = AnimeFactory.Anime(opt =>
                 {
                     opt.RequestParam = new AnimeRequestInput
                     {
                         AnimeType = AnimeEnum.Detail,
                         CacheSpan = CacheTime(),
                         Proxy = this.Proxy,
                         Detail = new AnimeDetail
                         {
                             DetailAddress = args
                         }
                     };
                 }).Runs();
                this.Detail = new ObservableCollection<AnimeDetailResult>(AnimeDetail.DetailResults);
            });
        }

        public void Category(string args)
        {
            SearchKey = string.Empty;
            CategoryKey = args;
            if (this.LetterCate.Contains(CategoryKey))
            {
                Task.Run(() =>
                {
                    var AnimeCate = AnimeFactory.Anime(opt =>
                    {
                        opt.RequestParam = new AnimeRequestInput
                        {
                            CacheSpan = CacheTime(),
                            AnimeType = AnimeEnum.Category,
                            Proxy = this.Proxy,
                            Category = new AnimeCategory
                            {
                                Page = PageIndex,
                                AnimeLetterType = Enum.Parse<AnimeLetterEnum>(CategoryKey)
                            }
                        };
                    }).Runs();
                    this.Total = AnimeCate.SeachResults.Page;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCate.SeachResults.Searchs);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    var AnimeCateType = AnimeFactory.Anime(opt =>
                {
                    opt.RequestParam = new AnimeRequestInput
                    {
                        AnimeType = AnimeEnum.CategoryType,
                        CacheSpan = CacheTime(),
                        Proxy = this.Proxy,
                        Category = new AnimeCategory
                        {
                            Page = PageIndex,
                            Address = CategoryKey
                        }
                    };
                }).Runs();
                    this.Total = AnimeCateType.SeachResults.Page;
                    this.Result = new ObservableCollection<AnimeSearchResults>(AnimeCateType.SeachResults.Searchs);
                });
            }
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            PageIndex = args.Info;
            if (CategoryKey.IsNullOrEmpty())
                SearchAnime(SearchKey);
            else
                Category(CategoryKey);
        }

        public void Play(string args)
        {
            var AnimeWath = AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new AnimeRequestInput
                {
                    AnimeType = AnimeEnum.Watch,
                    Proxy = new AnimeProxy(),
                    WatchPlay = new AnimeWatchPlay
                    {
                        DetailResult = Detail.Where(t => t.WatchAddress.Equals(args)).FirstOrDefault()
                    }
                };
            }).Runs();
          
            if (root.PlayBox == 0)
            {
                var vm = container.Get<AnimePlayWindowsVLCViewModel>();
                vm.WatchRoute = AnimeWath.PlayURL;
                AnimePlayWindowsByVLC win = null;
                if (VLC.ContainsKey(nameof(AnimePlayWindowsByVLC)))
                {
                    win = VLC[nameof(AnimePlayWindowsByVLC)];
                    win.CloseBase();
                    VLC.Clear();
                    win = new AnimePlayWindowsByVLC();
                    VLC[nameof(AnimePlayWindowsByVLC)] = win;
                    win.DataContext = vm;
                    win.Show();
                }
                else
                {
                    win = new AnimePlayWindowsByVLC();
                    VLC[nameof(AnimePlayWindowsByVLC)] = win;
                    win.DataContext = vm;
                    win.Show();
                }
            }
            if (root.PlayBox == 1)
            {
                var vm = container.Get<AnimePlayWindowsWebViewModel>();
                vm.WatchRoute = AnimeWath.PlayURL;
                AnimePlayWindowsByWEB win = null;
                if (DPlayer.ContainsKey(nameof(AnimePlayWindowsByWEB)))
                {
                    win = DPlayer[nameof(AnimePlayWindowsByWEB)];
                    win.Close();
                    DPlayer.Clear();
                    win = new AnimePlayWindowsByWEB();
                    DPlayer[nameof(AnimePlayWindowsByWEB)] = win;
                    win.DataContext = vm;
                    win.Show();
                }
                else {
                    win = new AnimePlayWindowsByWEB();
                    DPlayer[nameof(AnimePlayWindowsByWEB)] = win;
                    win.DataContext = vm;
                    win.Show();
                }
            }
        }
        #endregion
    }
}
