using Anime.SDK;
using Anime.SDK.ViewModel;
using Anime.SDK.ViewModel.Enums;
using Anime.SDK.ViewModel.Request;
using Anime.SDK.ViewModel.Response;
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

namespace Lote.Views.AnimeView
{
    public class AnimeViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly AnimeProxy Proxy;
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
        #endregion

        #region Method
        protected int CacheTime()
        {
            return root.CacheSpan.IsNullOrEmpty() ? 60 : Convert.ToInt32(root.CacheSpan);
        }
        protected override void OnViewLoaded()
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
        }
        public void SearchAnime(string args)
        { 
        
        }
        public void Redirect(string args)
        {
            var x = args;
        }
        #endregion
    }
}
