using HandyControl.Data;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Lote.NotifyUtil;
using Novel.SDK;
using Novel.SDK.ViewModel;
using Novel.SDK.ViewModel.Enums;
using Novel.SDK.ViewModel.Request;
using Novel.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace Lote.Views.NovelViews
{
    public class NovelViewModel : Screen
    {
        private readonly IContainer container;
        private readonly LoteSettingDTO root;
        private readonly NovelProxy Proxy;
        public NovelViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new LoteSettingDTO();
            this.Proxy = new NovelProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<NovelRecommendResult> _NovelRecommend;
        public ObservableCollection<NovelRecommendResult> NovelRecommend
        {
            get { return _NovelRecommend; }
            set { SetAndNotify(ref _NovelRecommend, value); }
        }

        private ObservableCollection<NovelCategoryResult> _NovelCategory;
        public ObservableCollection<NovelCategoryResult> NovelCategory
        {
            get { return _NovelCategory; }
            set { SetAndNotify(ref _NovelCategory, value); }
        }

        private ObservableCollection<NovelSearchResult> _NovelSearch;
        public ObservableCollection<NovelSearchResult> NovelSearch
        {
            get { return _NovelSearch; }
            set { SetAndNotify(ref _NovelSearch, value); }
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
        private string DetailAddress;
        private int Page;
        #endregion

        #region Method
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }

        protected override void OnViewLoaded()
        {
            var NovelInit = NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new NovelRequestInput
                {
                    CacheSpan = CacheTime(),
                    NovelType = NovelEnum.Init,
                    Proxy = this.Proxy
                };
            }).Runs();

            this.NovelCategory = new ObservableCollection<NovelCategoryResult>(NovelInit.IndexCategories);
            this.NovelRecommend = new ObservableCollection<NovelRecommendResult>(NovelInit.IndexRecommends);
        }

        public void SearchBook(string args)
        {
            Task.Run(() =>
            {
                var NovelSearch = NovelFactory.Novel(opt =>
                 {
                     opt.RequestParam = new NovelRequestInput
                     {
                         CacheSpan = CacheTime(),
                         NovelType = NovelEnum.Search,
                         Proxy = this.Proxy,
                         Search = new NovelSearch
                         {
                             NovelSearchKeyWord = args
                         }
                     };
                 }).Runs();
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelSearch.SearchResults);
            });
            this.Total = 0;
            this.Page = 1;
        }

        public void Redirect(string args)
        {
            this.DetailAddress = args;
            this.PageIndex = Page == 0 ? 1 : Page;
            Task.Run(() =>
            {
                var NovelCate = NovelFactory.Novel(opt =>
                 {
                     opt.RequestParam = new NovelRequestInput
                     {
                         CacheSpan = CacheTime(),
                         NovelType = NovelEnum.Category,
                         Proxy = this.Proxy,
                         Category = new NovelCategory
                         {
                             Page = this.PageIndex,
                             NovelCategoryAddress = args
                         }
                     };
                 }).Runs();
                this.Total = NovelCate.SingleCategories.TotalPage;
                this.NovelSearch = new ObservableCollection<NovelSearchResult>(NovelCate.SingleCategories.NovelSingles.ToMapest<List<NovelSearchResult>>());
            });

        }

        public void PageUpdated(FunctionEventArgs<int> args)
        {
            Page = args.Info;
            Redirect(this.DetailAddress);
        }

        public void GetBook(dynamic entity)
        {
            var NovelDetail = NovelFactory.Novel(opt =>
            {
                opt.RequestParam = new NovelRequestInput
                {
                    CacheSpan = CacheTime(),
                    NovelType = NovelEnum.Detail,
                    Proxy = this.Proxy,
                    Detail = new NovelDetail
                    {
                        NovelDetailAddress = entity.DetailAddress
                    }
                };
            }).Runs();
            var vm = container.Get<NovelContentViewModel>();
            vm.NovelDetail = NovelDetail.Details;
            vm.PageIndex = 1;
            vm.Addr = entity.DetailAddress;
            container.Get<INavigationController>().Delegate.NavigateTo(vm);
        }
        #endregion
    }
}
