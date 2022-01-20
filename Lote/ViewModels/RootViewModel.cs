using Lote.Core.Common;
using Lote.NotifyUtil;
using Lote.ViewModels.DTO;
using Lote.Views;
using Lote.Views.AnimeViews;
using Lote.Views.LightNovelViews;
using Lote.Views.MangaViews;
using Lote.Views.MusicViews;
using Lote.Views.NovelViews;
using Lote.Views.OptionViews;
using Lote.Views.UserCenterViews;
using Lote.Views.WallpaperViews;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using XExten.Advance.LinqFramework;

namespace Lote.ViewModels
{
    public class RootViewModel : Conductor<IScreen>, IActiveNotify
    {
        private readonly IContainer container;
        public RootViewModel(IContainer container)
        {
            this.container = container;
            this.Nav = new RootViewDTO().Datas();
            this.Soft = $"轻风神器Ver_{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private ObservableCollection<RootViewDTO> _Nav;
        public ObservableCollection<RootViewDTO> Nav
        {
            get { return _Nav; }
            set { SetAndNotify(ref _Nav, value); }
        }

        private string _Soft;
        public string Soft
        {
            get { return _Soft; }
            set { SetAndNotify(ref _Soft, value); }
        }

        public void NavigateTo(IScreen screen)
        {
            ActivateItem(screen);
        }

        public void Redirect(string args)
        {
            var type = (MenuFuncEunm)args.AsInt();

            switch (type)
            {
                case MenuFuncEunm.Novel:
                    NavigateTo(container.Get<NovelViewModel>());
                    break;
                case MenuFuncEunm.LightNovel:
                    NavigateTo(container.Get<LightNovelViewModel>());
                    break;
                case MenuFuncEunm.Anime:
                    NavigateTo(container.Get<AnimeViewModel>());
                    break;
                case MenuFuncEunm.Manga:
                    NavigateTo(container.Get<MangaViewModel>());
                    break;
                case MenuFuncEunm.Wallpaper:
                    var res = HandyControl.Controls.MessageBox.Show("未满18周岁请勿点击，部分内容可能引起不适", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (res == MessageBoxResult.Yes)
                        NavigateTo(container.Get<WallpaperViewModel>());
                    break;
                case MenuFuncEunm.Music:
                    NavigateTo(container.Get<MusicViewModel>());
                    break;
                case MenuFuncEunm.Setting:
                    NavigateTo(container.Get<OptionViewModel>());
                    break;
                case MenuFuncEunm.UserCenter:
                    if (Application.Current.MainWindow.Visibility == Visibility.Collapsed)
                        Application.Current.MainWindow.Visibility = Visibility.Visible;
                    NavigateTo(container.Get<UserCenterViewModel>());
                    break;
                default:
                    break;
            }
        }

        public void ShowWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
        }
    }
}
