using Anime.SDK.ViewModel.Response;
using Lote.CommonWindow;
using Lote.CommonWindow.ViewMdeol;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Manga.SDK.ViewModel.Response;
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

namespace Lote.Views.UserCenterViews
{
    public class UserCenterViewModel : Screen
    {
        private readonly IContainer container;
        public UserCenterViewModel(IContainer container)
        {
            this.container = container;
        }
        private ObservableCollection<LoteNovelHistoryDTO> _NovelHistories;
        public ObservableCollection<LoteNovelHistoryDTO> NovelHistories
        {
            get { return _NovelHistories; }
            set { SetAndNotify(ref _NovelHistories, value); }
        }

        private ObservableCollection<LoteMangaHistoryDTO> _MangaHistories;
        public ObservableCollection<LoteMangaHistoryDTO> MangaHistories
        {
            get { return _MangaHistories; }
            set { SetAndNotify(ref _MangaHistories, value); }
        }

        private ObservableCollection<LoteAnimeHistoryDTO> _AnimeHistories;
        public ObservableCollection<LoteAnimeHistoryDTO> AnimeHistories
        {
            get { return _AnimeHistories; }
            set { SetAndNotify(ref _AnimeHistories, value); }
        }

        protected override void OnViewLoaded()
        {
            NovelHistories = new ObservableCollection<LoteNovelHistoryDTO>(container.Get<IHistoryService>().GetNovelHistory());
            MangaHistories = new ObservableCollection<LoteMangaHistoryDTO>(container.Get<IHistoryService>().GetMangaHistory());
            AnimeHistories = new ObservableCollection<LoteAnimeHistoryDTO>(container.Get<IHistoryService>().GetAnimeHistory());
        }

        public void Play(LoteAnimeHistoryDTO args)
        {
            if (args.PlayMode)
            {
                var vm = container.Get<AnimePlayWindowsWebViewModel>();
                vm.WatchResult = args.ToMapest<AnimePlayResult>();
                //Open
                BootResource.AnimeWEB(window =>
                {
                    window.DataContext = vm;
                });
            }
            else
            {
                var vm = container.Get<AnimePlayWindowsVLCViewModel>();
                vm.WatchResult = args.ToMapest<AnimePlayResult>();
                //Open
                BootResource.AnimeVLC(window =>
                {
                    window.DataContext = vm;
                });
            }
        }

        public void Watch(LoteMangaHistoryDTO args)
        {
            var vm = container.Get<MangaReaderWindowsViewModel>();
            vm.Loading = true;
            vm.Chapters = args.Chapters.ToModel<ObservableCollection<MangaChapterResult>>();
            vm.Total = vm.Chapters.Count;
            vm.Index = args.Index;
            vm.InitCurrent();
            //Open
            BootResource.Manga(window =>
            {
                window.DataContext = vm;
            });
        }

        public void Reader(LoteNovelHistoryDTO args)
        {
            var vm = container.Get<NovelContentWindowsViewModel>();
            vm.NovelContent = args.ToMapest<NovelContentResult>();
            vm.BookName = args.BookName;
            //Open
            BootResource.Novel(window =>
            {
                window.DataContext = vm;
            });
        }
    }
}
