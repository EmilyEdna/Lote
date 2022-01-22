using Lote.CommonWindow;
using Lote.Override;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lote
{
    public  class BootResource
    {
        private static ConcurrentDictionary<string, NovelContentWindows> NovelContentWindow = new ConcurrentDictionary<string, NovelContentWindows>();
        private static ConcurrentDictionary<string, AnimePlayWindowsByVLC> AnimePlayWindowVLC= new ConcurrentDictionary<string, AnimePlayWindowsByVLC>();
        private static ConcurrentDictionary<string, AnimePlayWindowsByWEB> AnimePlayWindowDPlayer = new ConcurrentDictionary<string, AnimePlayWindowsByWEB>();
        private static ConcurrentDictionary<string, WallpaperPreviewWindows> WallpaperPreviewWindow = new ConcurrentDictionary<string, WallpaperPreviewWindows>();
        private static ConcurrentDictionary<string, LightNovelContentWindows> LightNovelContentWindow= new ConcurrentDictionary<string, LightNovelContentWindows>();
        private static ConcurrentDictionary<string, MangaReaderWindows> MangaReaderWindow= new ConcurrentDictionary<string, MangaReaderWindows>();
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void LightNovel(Action<LightNovelContentWindows> action)
        {
            LightNovelContentWindows windows = null;
            if (LightNovelContentWindow.ContainsKey(nameof(LightNovelContentWindows)))
            {
                var old = LightNovelContentWindow.Values.FirstOrDefault();
                old.Close();
                LightNovelContentWindow.Clear();
                windows = new LightNovelContentWindows();
                action(windows);
                windows.Show();
            }
            else
            {
                LightNovelContentWindow.Clear();
                windows = new LightNovelContentWindows();
                action(windows);
                LightNovelContentWindow.TryAdd(nameof(LightNovelContentWindows), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Novel(Action<NovelContentWindows> action)
        {
            NovelContentWindows windows = null;
            if (NovelContentWindow.ContainsKey(nameof(NovelContentWindows)))
            {
                var old = NovelContentWindow.Values.FirstOrDefault();
                old.Close();
                NovelContentWindow.Clear();
                windows = new NovelContentWindows();
                action(windows);
                NovelContentWindow.TryAdd(nameof(NovelContentWindows), windows);
                windows.Show();
            }
            else
            {
                NovelContentWindow.Clear();
                windows = new NovelContentWindows();
                action(windows);
                NovelContentWindow.TryAdd(nameof(NovelContentWindows), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Wallpaper(Action<WallpaperPreviewWindows> action)
        {
            WallpaperPreviewWindows windows = null;
            if (WallpaperPreviewWindow.ContainsKey(nameof(WallpaperPreviewWindows)))
            {
                var old = WallpaperPreviewWindow.Values.FirstOrDefault();
                old.Close();
                WallpaperPreviewWindow.Clear();
                windows = new WallpaperPreviewWindows();
                action(windows);
                WallpaperPreviewWindow.TryAdd(nameof(WallpaperPreviewWindows), windows);
                windows.Show();
            }
            else
            {
                WallpaperPreviewWindow.Clear();
                windows = new WallpaperPreviewWindows();
                action(windows);
                WallpaperPreviewWindow.TryAdd(nameof(WallpaperPreviewWindows), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void AnimeWEB(Action<AnimePlayWindowsByWEB> action)
        {
            AnimePlayWindowsByWEB windows = null;
            if (AnimePlayWindowDPlayer.ContainsKey(nameof(AnimePlayWindowsByWEB)))
            {
                var old = AnimePlayWindowDPlayer.Values.FirstOrDefault();
                old.Close();
                AnimePlayWindowDPlayer.Clear();
                windows = new AnimePlayWindowsByWEB();
                action(windows);
                AnimePlayWindowDPlayer.TryAdd(nameof(AnimePlayWindowsByWEB), windows);
                windows.Show();
            }
            else
            {
                AnimePlayWindowDPlayer.Clear();
                windows = new AnimePlayWindowsByWEB();
                action(windows);
                AnimePlayWindowDPlayer.TryAdd(nameof(AnimePlayWindowsByWEB), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void AnimeVLC(Action<AnimePlayWindowsByVLC> action)
        {
            AnimePlayWindowsByVLC windows = null;
            if (AnimePlayWindowVLC.ContainsKey(nameof(LightNovelContentWindows)))
            {
                var old = AnimePlayWindowVLC.Values.FirstOrDefault();
                old.CloseBase();
                AnimePlayWindowVLC.Clear();
                windows = new AnimePlayWindowsByVLC();
                action(windows);
                AnimePlayWindowVLC.TryAdd(nameof(AnimePlayWindowsByVLC), windows);
                windows.Show();
            }
            else
            {
                AnimePlayWindowVLC.Clear();
                windows = new AnimePlayWindowsByVLC();
                action(windows);
                AnimePlayWindowVLC.TryAdd(nameof(AnimePlayWindowsByVLC), windows);
                windows.Show();
            }
        }
        /// <summary>
        /// 控制窗体打开
        /// </summary>
        /// <param name="action"></param>
        public static void Manga(Action<MangaReaderWindows> action)
        {
            MangaReaderWindows windows = null;
            if (MangaReaderWindow.ContainsKey(nameof(MangaReaderWindows)))
            {
                var old = MangaReaderWindow.Values.FirstOrDefault();
                old.Close();
                MangaReaderWindow.Clear();
                windows = new MangaReaderWindows();
                action(windows);
                MangaReaderWindow.TryAdd(nameof(MangaReaderWindows), windows);
                windows.Show();
            }
            else
            {
                MangaReaderWindow.Clear();
                windows = new MangaReaderWindows();
                action(windows);
                MangaReaderWindow.TryAdd(nameof(MangaReaderWindows), windows);
                windows.Show();
            }
        }
    }
}
