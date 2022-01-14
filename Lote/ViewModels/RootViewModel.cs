using Lote.Core.Common;
using Lote.NotifyUtil;
using Lote.Views.AnimeView;
using Lote.Views.LightNovelView;
using Lote.Views.Music;
using Lote.Views.NovelView;
using Lote.Views.Option;
using Lote.Views.Wallpaper;
using Stylet;
using StyletIoC;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XExten.Advance.LinqFramework;

namespace Lote.ViewModels
{
    public class RootViewModel : Conductor<IScreen>, IActiveNotify
    {
        private readonly IContainer container;
        public RootViewModel(IContainer container)
        {
            this.container = container;
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
                default:
                    break;
            }
        }
    }
}
