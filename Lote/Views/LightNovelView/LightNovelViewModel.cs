using HandyControl.Controls;
using HandyControl.Data;
using LightNovel.SDK;
using LightNovel.SDK.ViewModel;
using LightNovel.SDK.ViewModel.Enums;
using LightNovel.SDK.ViewModel.Request;
using LightNovel.SDK.ViewModel.Response;
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
using System.Windows.Controls;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Views.LightNovelView
{
    public class LightNovelViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly LightNovelProxy Proxy;
        public LightNovelViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new LightNovelProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<LightNovelCategoryResult> _LightNovelCategory;
        public ObservableCollection<LightNovelCategoryResult> LightNovelCategory
        {
            get { return _LightNovelCategory; }
            set { SetAndNotify(ref _LightNovelCategory, value); }
        }

        private ObservableCollection<LightNovelSingleCategoryResults> _LightNovelSingleCategory;
        public ObservableCollection<LightNovelSingleCategoryResults> LightNovelSingleCategory
        {
            get { return _LightNovelSingleCategory; }
            set { SetAndNotify(ref _LightNovelSingleCategory, value); }
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
        private bool IsSearch = false;
        private string SearchWord;
        private string CategoryAddress;
        private LightNovelSearchEnum SearchType;
        #endregion

        #region Method

        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }
        protected Dictionary<string, string> WkInfo()
        {
            if (root.UseAuthorWKInfo)
                return new Dictionary<string, string> { { root.WkAccount.IsNullOrEmpty() ? "-" : root.WkAccount, root.WkPwd.IsNullOrEmpty() ? "-" : root.WkPwd } };
            else
                return new Dictionary<string, string> { { "kilydoll365", "sion8550" } };
        }
        protected override void OnViewLoaded()
        {
            SyncStatic.TryCatch(() =>
            {
                //初始化
                var LightNovelInit = LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        CacheSpan = CacheTime(),
                        LightNovelType = LightNovelEnum.Init,
                        Proxy = this.Proxy
                    };
                }).Runs(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, this.Proxy);
                });
                LightNovelCategory = new ObservableCollection<LightNovelCategoryResult>(LightNovelInit.CategoryResults);
                CategoryAddress = LightNovelInit.CategoryResults.FirstOrDefault().CategoryAddress;
                //分类
                var LightNovelCate = LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Category,
                        CacheSpan = CacheTime(),
                        Proxy = this.Proxy,
                        Category = new LightNovelCategory
                        {
                            CategoryAddress = CategoryAddress
                        }
                    };
                }).Runs(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });
                LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                Total = LightNovelCate.SingleCategoryResult.TotalPage;
                PageIndex = 1;
                IsSearch = false;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Error("服务异常，请稍后重试", "错误");
            });
        }

        public void SearchBook(string args)
        {
            SearchWord = args;
            SyncStatic.TryCatch(() =>
            {
                //搜索
                var LightNovelSearch = LightNovelFactory.LightNovel(opt =>
                 {
                     opt.RequestParam = new LightNovelRequestInput
                     {
                         LightNovelType = LightNovelEnum.Search,
                         CacheSpan = CacheTime(),
                         Proxy = this.Proxy,
                         Search = new LightNovelSearch
                         {
                             KeyWord = args,
                             SearchType = SearchType
                         }
                     };
                 }).Runs(Light =>
                 {
                     Light.RefreshCookie(new LightNovelRefresh
                     {
                         UserName = WkInfo().Keys.FirstOrDefault(),
                         PassWord = WkInfo().Values.FirstOrDefault()
                     }, new LightNovelProxy());
                 });

                LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>());
                Total = LightNovelSearch.SearchResults.TotalPage;
                PageIndex = 1;
                IsSearch = true;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Error("服务异常，请稍后重试", "错误");
            });
        }

        public void Redirect(string args)
        {
            CategoryAddress = args;
            SyncStatic.TryCatch(() =>
            {
                var LightNovelCate = LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Category,
                        CacheSpan = CacheTime(),
                        Proxy = this.Proxy,
                        Category = new LightNovelCategory
                        {
                            CategoryAddress = args
                        }
                    };
                }).Runs(Light =>
                {
                    Light.RefreshCookie(new LightNovelRefresh
                    {
                        UserName = WkInfo().Keys.FirstOrDefault(),
                        PassWord = WkInfo().Values.FirstOrDefault()
                    }, new LightNovelProxy());
                });
                LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                Total = LightNovelCate.SingleCategoryResult.TotalPage;
                PageIndex = 1;
                IsSearch = false;
            }, ex =>
            {
                LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        LightNovelType = LightNovelEnum.Refresh
                    };
                }).Runs();
                MessageBox.Error("服务异常，请稍后重试", "错误");
            });
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            if (IsSearch)
            {
                SyncStatic.TryCatch(() =>
                {
                    PageIndex = args.Info;
                    //搜索
                    var LightNovelSearch = LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Search,
                            CacheSpan = CacheTime(),
                            Proxy = this.Proxy,
                            Search = new LightNovelSearch
                            {
                                Page = args.Info,
                                KeyWord = SearchWord,
                                SearchType = SearchType
                            }
                        };
                    }).Runs(Light =>
                    {
                        Light.RefreshCookie(new LightNovelRefresh
                        {
                            UserName = WkInfo().Keys.FirstOrDefault(),
                            PassWord = WkInfo().Values.FirstOrDefault()
                        }, new LightNovelProxy());
                    });

                    LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelSearch.SearchResults.Result.ToMapest<List<LightNovelSingleCategoryResults>>());
                    Total = LightNovelSearch.SearchResults.TotalPage;

                }, ex =>
                {
                    LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Refresh
                        };
                    }).Runs();
                    MessageBox.Error("服务异常，请稍后重试", "错误");
                });
            }
            else {
                PageIndex = args.Info;
                SyncStatic.TryCatch(() =>
                {
                    var LightNovelCate = LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Category,
                            CacheSpan = CacheTime(),
                            Proxy = this.Proxy,
                            Category = new LightNovelCategory
                            {
                                Page = args.Info,
                                CategoryAddress = CategoryAddress
                            }
                        };
                    }).Runs(Light =>
                    {
                        Light.RefreshCookie(new LightNovelRefresh
                        {
                            UserName = WkInfo().Keys.FirstOrDefault(),
                            PassWord = WkInfo().Values.FirstOrDefault()
                        }, new LightNovelProxy());
                    });
                    LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
                    Total = LightNovelCate.SingleCategoryResult.TotalPage;
                    IsSearch = false;
                }, ex =>
                {
                    LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Refresh
                        };
                    }).Runs();
                    MessageBox.Error("服务异常，请稍后重试", "错误");
                });
            }
        }

        public void GetBook(LightNovelSingleCategoryResults entity)
        {
            

        }

        public void SetSearchType(ComboBoxItem control) 
        {
            SearchType = (LightNovelSearchEnum)control.Tag.ToString().AsInt();
        }
        #endregion
    }
}
