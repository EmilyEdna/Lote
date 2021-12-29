using HandyControl.Controls;
using HandyControl.Data;
using LightNovel.SDK;
using LightNovel.SDK.ViewModel;
using LightNovel.SDK.ViewModel.Enums;
using LightNovel.SDK.ViewModel.Request;
using LightNovel.SDK.ViewModel.Response;
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly IDictionary<string, LightNovelContentWindows> data;
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
            this.data = new Dictionary<string, LightNovelContentWindows>();
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

        private ObservableCollection<LightNovelViewResult> _LightNovelViews;
        public ObservableCollection<LightNovelViewResult> LightNovelViews
        {
            get { return _LightNovelViews; }
            set { SetAndNotify(ref _LightNovelViews, value); }
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
        private int Page;
        private string BookName;
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
                Task.Run(() =>
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
                });
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
            this.PageIndex = Page == 0 ? 1 : Page;
            SyncStatic.TryCatch(() =>
            {
                Task.Run(() =>
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
                                 Page = this.PageIndex,
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
                });
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
            this.PageIndex = Page == 0 ? 1 : Page;
            SyncStatic.TryCatch(() =>
            {
                Task.Run(() =>
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
                                Page = this.PageIndex,
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
                });
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
                Page = args.Info;
                SearchBook(SearchWord);
            }
            else
            {
                Page = args.Info;
                Redirect(CategoryAddress);
            }
        }

        public void GetBook(LightNovelSingleCategoryResults entity)
        {
            BookName = entity.BookName;

            SyncStatic.TryCatch(() =>
            {
                Task.Run(() =>
                {
                    var LightNovelDetail = LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Detail,
                            CacheSpan = CacheTime(),
                            Proxy = this.Proxy,
                            Detail = new LightNovelDetail
                            {
                                DetailAddress = entity.DetailAddress
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

                    var LightNovelView = LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.View,
                            Proxy = new LightNovelProxy(),
                            CacheSpan = CacheTime(),
                            View = new LightNovel.SDK.ViewModel.Request.LightNovelView
                            {
                                ViewAddress = LightNovelDetail.DetailResult.Address,
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

                    LightNovelViews = new ObservableCollection<LightNovelViewResult>(LightNovelView.ViewResult);
                });
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

        public void SetSearchType(ComboBoxItem control)
        {
            SearchType = (LightNovelSearchEnum)control.Tag.ToString().AsInt();
        }

        public void GetContent(LightNovelViewResult entity)
        {
            SyncStatic.TryCatch(() =>
            {
                //内容
                var LightNovelContent = LightNovelFactory.LightNovel(opt =>
                {
                    opt.RequestParam = new LightNovelRequestInput
                    {
                        CacheSpan = CacheTime(),
                        LightNovelType = LightNovelEnum.Content,
                        Proxy = this.Proxy,
                        Content = new LightNovelContent
                        {
                            ChapterURL = entity.ChapterURL,
                        }
                    };
                }).Runs();

                if (DownNovel(entity.ChapterURL, LightNovelContent.ContentResult.Content) == false)
                    return;

                var vm = container.Get<LightNovelContentWindowsViewModel>();
                vm.LightNovelContent = LightNovelContent.ContentResult;
                vm.Show = LightNovelContent.ContentResult.Image == null;
                LightNovelContentWindows win = null;
                if (data.ContainsKey(nameof(LightNovelContentWindows)))
                {
                    win = data[nameof(LightNovelContentWindows)];
                    win.DataContext = vm;
                }
                else
                {
                    win = new LightNovelContentWindows();
                    data[nameof(NovelContentWindows)] = win;
                    win.DataContext = vm;
                    win.Show();
                }

            }, ex => MessageBox.Error("服务异常，请稍后重试", "错误"));

        }

        private bool DownNovel(string Url, string Check)
        {
            if (Check.Equals("因版权问题，文库不再提供该小说的阅读！"))
            {
                var result = MessageBox.Show("因版权问题，文库不再提供该小说的阅读！是否下载阅读？", "提示",
                      System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);

                var UId = Regex.Matches(Url, "[0-9]+/").LastOrDefault().Value.Split("/").FirstOrDefault().AsInt();

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    var LightNovelDown = LightNovelFactory.LightNovel(opt =>
                    {
                        opt.RequestParam = new LightNovelRequestInput
                        {
                            LightNovelType = LightNovelEnum.Down,
                            Proxy = this.Proxy,
                            Down = new LightNovelDown
                            {
                                UId = UId,
                                BookName = BookName
                            }
                        };
                    }).Runs();

                    var dir = FileUtily.Instance.DownFile(LightNovelDown.DownResult.Down, "LightNovel", $"{BookName}.txt");
                    MessageBox.Info("下载完成", "提示");
                    Process.Start("explorer.exe", dir);
                    return false;
                }
                return false;
            }
            return true;
        }
        #endregion
    }
}
