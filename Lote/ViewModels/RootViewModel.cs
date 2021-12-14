using Lote.NotifyUtil;
using Lote.Views.AnimeView;
using Lote.Views.LightNovelView;
using Lote.Views.Music;
using Lote.Views.NovelView;
using Lote.Views.Option;
using Lote.Views.Wallpaper;
using Stylet;
using StyletIoC;
using System.Windows.Controls;
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
            var type = args.AsInt();
            if (type == 1)
                NavigateTo(container.Get<NovelViewModel>());
            if (type == 2)
                NavigateTo(container.Get<LightNovelViewModel>());
            if (type == 3)
                NavigateTo(container.Get<AnimeViewModel>());
            if (type == 4)
                NavigateTo(container.Get<WallpaperViewModel>());
            if (type == 5)
                NavigateTo(container.Get<MusicViewModel>());
            if (type == 6)
                NavigateTo(container.Get<OptionViewModel>());
        }
    }
}
