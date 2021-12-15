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
using XExten.Advance.LinqFramework;

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
                IP = root == null ? String.Empty : root.ProxyIP,
                PassWord = root == null ? String.Empty : root.ProxyPwd,
                Port = root == null ? -1 : Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root == null ? String.Empty : root.ProxyAccount
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

        #region Method
        protected override void OnViewLoaded()
        {
            //初始化
            var LightNovelInit = LightNovelFactory.LightNovel(opt =>
            {
                opt.RequestParam = new LightNovelRequestInput
                {
                    CacheSpan = root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan),
                    LightNovelType = LightNovelEnum.Init,
                    Proxy = this.Proxy
                };
            }).Runs(Light =>
            {
                Light.RefreshCookie(new LightNovelRefresh
                {
                    UserName = this.root == null || this.root.WkAccount.IsNullOrEmpty() ? "kilydoll365" : this.root.WkAccount,
                    PassWord = this.root == null || this.root.WkPwd.IsNullOrEmpty() ? "sion8550" : this.root.WkPwd
                }, this.Proxy);
            });
            LightNovelCategory = new ObservableCollection<LightNovelCategoryResult>(LightNovelInit.CategoryResults);
            //分类
            var LightNovelCate = LightNovelFactory.LightNovel(opt =>
            {
                opt.RequestParam = new LightNovelRequestInput
                {
                    LightNovelType = LightNovelEnum.Category,
                    CacheSpan = root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan),
                    Proxy = this.Proxy,
                    Category = new LightNovelCategory
                    {
                        CategoryAddress = LightNovelInit.CategoryResults.FirstOrDefault().CategoryAddress
                    }
                };
            }).Runs(Light =>
            {
                Light.RefreshCookie(new LightNovelRefresh
                {
                    UserName = this.root == null || this.root.WkAccount.IsNullOrEmpty() ? "kilydoll365" : this.root.WkAccount,
                    PassWord = this.root == null || this.root.WkPwd.IsNullOrEmpty() ? "sion8550" : this.root.WkPwd
                }, new LightNovelProxy());
            });
            LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
            Total = LightNovelCate.SingleCategoryResult.TotalPage;
            PageIndex = 1;
        }

        public void SearchBook(string args)
        {
            //搜索
            var LightNovelSearch = LightNovelFactory.LightNovel(opt =>
            {
                opt.RequestParam = new LightNovelRequestInput
                {
                    LightNovelType = LightNovelEnum.Search,
                    CacheSpan = root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan),
                    Proxy = this.Proxy,
                    Search = new LightNovelSearch
                    {
                        KeyWord = args,
                        SearchType = LightNovelSearchEnum.ArticleName
                    }
                };
            }).Runs(Light =>
            {
                Light.RefreshCookie(new LightNovelRefresh
                {
                    UserName = this.root == null || this.root.WkAccount.IsNullOrEmpty() ? "kilydoll365" : this.root.WkAccount,
                    PassWord = this.root == null || this.root.WkPwd.IsNullOrEmpty() ? "sion8550" : this.root.WkPwd
                }, new LightNovelProxy());
            });

            Total = LightNovelSearch.SearchResults.TotalPage;
        }

        public void Redirect(string args)
        {
            var LightNovelCate = LightNovelFactory.LightNovel(opt =>
            {
                opt.RequestParam = new LightNovelRequestInput
                {
                    LightNovelType = LightNovelEnum.Category,
                    CacheSpan = root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan),
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
                    UserName = this.root == null || this.root.WkAccount.IsNullOrEmpty() ? "kilydoll365" : this.root.WkAccount,
                    PassWord = this.root == null || this.root.WkPwd.IsNullOrEmpty() ? "sion8550" : this.root.WkPwd
                }, new LightNovelProxy());
            });
            LightNovelSingleCategory = new ObservableCollection<LightNovelSingleCategoryResults>(LightNovelCate.SingleCategoryResult.Result);
            Total = LightNovelCate.SingleCategoryResult.TotalPage;
            PageIndex = 1;
        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {

        }

        public void GetBook(string args) 
        { 
        
        }
        #endregion
    }
}
