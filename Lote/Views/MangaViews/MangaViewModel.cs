using HandyControl.Controls;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Manga.SDK;
using Manga.SDK.ViewModel;
using Manga.SDK.ViewModel.Emums;
using Manga.SDK.ViewModel.Request;
using Manga.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace Lote.Views.MangaViews
{
    public class MangaViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly MangaProxy Proxy;

        public MangaViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new MangaProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<MangaRecommendResult> _MangaRecommend;
        public ObservableCollection<MangaRecommendResult> MangaRecommend
        {
            get { return _MangaRecommend; }
            set { SetAndNotify(ref _MangaRecommend, value); }
        }

        private ObservableCollection<MangaCategoryResult> _MangaCategory;
        public ObservableCollection<MangaCategoryResult> MangaCategory
        {
            get { return _MangaCategory; }
            set { SetAndNotify(ref _MangaCategory, value); }
        }

        private ObservableCollection<MangaChapterResult> _Chapters;
        public ObservableCollection<MangaChapterResult> Chapters
        {
            get { return _Chapters; }
            set { SetAndNotify(ref _Chapters, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }
        #endregion

        #region Field
        public int Type { get; set; }
        private string Keyword = string.Empty;
        #endregion

        #region Method
        public void SearchManga(string args)
        {
            this.Type = 1;
            this.PageIndex = 1;
            Keyword = args;
            Handle();
            Chapters = new ObservableCollection<MangaChapterResult>();
        }

        public void Redirect(string args)
        {
            this.Type = 2;
            this.PageIndex = 1;
            Keyword = args;
            Handle();
            Chapters = new ObservableCollection<MangaChapterResult>();
        }

        public void GetManga(MangaRecommendResult input)
        {
            if (input == null)
                return;
            Task.Run(() =>
            {
                //详情
                var MangaDetail = MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.Detail,
                        Proxy = new MangaProxy(),
                        Detail = new MangaDetail
                        {
                            Address = input.Address
                        }
                    };
                }).Runs();

                Chapters = new ObservableCollection<MangaChapterResult>(MangaDetail.ChapterResults);
            });
        }

        public void GetContent(MangaChapterResult input)
        {
            if (Chapters.Count != 0)
            {
                Chapters.Where(t => t.TagKey == input.TagKey).ToList();
            }
        }

        public void LoadMore(bool types)
        {
            if (Type == 0 || Keyword.IsNullOrEmpty())
                return;
            int NextPage = types ? PageIndex += 1 : PageIndex -= 1;
            if (NextPage < 0) return;
            Handle();
        }
        #endregion

        protected void Handle()
        {
            if (Type == 1)
            {
                Task.Run(() =>
                {
                    //检索
                    var MangaSearch = MangaFactory.Manga(opt =>
                    {
                        opt.RequestParam = new MangaRequestInput
                        {
                            MangaType = MangaEnum.Search,
                            Proxy = new MangaProxy(),
                            Search = new MangaSearch
                            {
                                KeyWord = Keyword,
                                Page = PageIndex
                            }
                        };
                    }).Runs();
                    if (MangaSearch.SearchResults.Count == 0)
                        MessageBox.Info("数据已到底~`(*>﹏<*)′", "提示");
                    else
                        MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaSearch.SearchResults.ToMapest<List<MangaRecommendResult>>());
                });
            }
            else
            {
                Task.Run(() =>
                {
                    //分类
                    var MangaCate = MangaFactory.Manga(opt =>
                    {
                        opt.RequestParam = new MangaRequestInput
                        {
                            MangaType = MangaEnum.Category,
                            Proxy = new MangaProxy(),
                            Category = new MangaCategory
                            {
                                Address = Keyword,
                                Page = PageIndex
                            }
                        };
                    }).Runs();
                    if (MangaCate.SearchResults.Count == 0)
                        MessageBox.Info("数据已到底~`(*>﹏<*)′", "提示");
                    else
                        MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaCate.SearchResults.ToMapest<List<MangaRecommendResult>>());
                });
            }
        }
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }
        protected override void OnViewLoaded()
        {
            PageIndex = 1;
            Task.Run(() =>
            {
                //初始化
                var MangaInit = MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new MangaRequestInput
                    {
                        MangaType = MangaEnum.Init,
                        Proxy = new MangaProxy(),
                        CacheSpan = CacheTime(),
                    };
                }).Runs();

                MangaRecommend = new ObservableCollection<MangaRecommendResult>(MangaInit.IndexRecommends);
                MangaCategory = new ObservableCollection<MangaCategoryResult>(MangaInit.IndexCategories);
            });
        }
    }
}
