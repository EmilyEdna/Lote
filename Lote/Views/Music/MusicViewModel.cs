using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Music.SDK;
using Music.SDK.ViewModel;
using Music.SDK.ViewModel.Enums;
using Music.SDK.ViewModel.Request;
using Music.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Views.Music
{
    public class MusicViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly MusicProxy Proxy;
        private readonly IMusicPlayService MusciService;
        public MusicViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.MusciService = container.Get<IMusicPlayService>();
            this.Proxy = new MusicProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
            this.PageIndex = 1;
            this.Platform = MusicPlatformEnum.NeteaseMusic;
        }

        #region Field
        private string KeyWord;
        private int ShowType;
        #endregion

        #region Property
        private ObservableCollection<MusicSongItem> _SongItems;
        public ObservableCollection<MusicSongItem> SongItems
        {
            get { return _SongItems; }
            set { SetAndNotify(ref _SongItems, value); }
        }

        private ObservableCollection<MusicSongSheetItem> _SongSheets;
        public ObservableCollection<MusicSongSheetItem> SongSheets
        {
            get { return _SongSheets; }
            set { SetAndNotify(ref _SongSheets, value); }
        }

        private MusicSongSheetDetailResult _SheetDetail;
        public MusicSongSheetDetailResult SheetDetail
        {
            get { return _SheetDetail; }
            set { SetAndNotify(ref _SheetDetail, value); }
        }

        public ObservableCollection<PlayListDTO> _PlayLists;
        public ObservableCollection<PlayListDTO> PlayLists
        {
            get { return _PlayLists; }
            set { SetAndNotify(ref _PlayLists, value); }
        }

        private MusicSongAlbumDetailResult _AlbumDetail;
        public MusicSongAlbumDetailResult AlbumDetail
        {
            get { return _AlbumDetail; }
            set { SetAndNotify(ref _AlbumDetail, value); }
        }

        private int _Total;
        public int Total
        {
            get { return _Total; }
            set { SetAndNotify(ref _Total, value); }
        }

        private int _Count;
        public int Count
        {
            get { return _Count; }
            set { SetAndNotify(ref _Count, value); }
        }

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { SetAndNotify(ref _PageIndex, value); }
        }

        private MusicPlatformEnum _Platform;
        public MusicPlatformEnum Platform
        {
            get { return _Platform; }
            set { SetAndNotify(ref _Platform, value); }
        }
        #endregion

        #region Method
        public void SearchType(ComboBoxItem control)
        {
            this.Platform = (MusicPlatformEnum)control.Tag.ToString().AsInt();
            SheetDetail = null;
            AlbumDetail = null;
        }
        public void SearchMusic(string args)
        {
            KeyWord = args;
            Task.Run(() => Search());
        }
        public void ShowSong(int args)
        {
            if (KeyWord.IsNullOrEmpty())
                return;

            if (args == 1 || args == 2)
                Task.Run(() => Search(args));
        }
        public void SeleteSheet(MusicSongSheetItem entity)
        {
            if (entity == null)
                return;
            var SheetDetail = MusicFactory.Music(opt =>
            {
                opt.RequestParam = new MusicRequestInput
                {
                    Proxy = this.Proxy,
                    MusicPlatformType = Platform,
                    MusicType = MusicTypeEnum.SheetDetail,
                    SheetSearch = new MusicSheetSearch
                    {
                        Page = PageIndex,
                        Id = entity.SongSheetId.AsString()
                    }
                };
            }).Runs();
            this.SheetDetail = SheetDetail.SongSheetDetailResult;
        }
        public void ShowAlbum(string args)
        {
            var SongAlbum = MusicFactory.Music(opt =>
            {
                opt.RequestParam = new MusicRequestInput
                {
                    MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                    MusicType = MusicTypeEnum.AlbumDetail,
                    AlbumSearch = new MusicAlbumSearch
                    {
                        AlbumId = args
                    }
                };
            }).Runs();
            this.AlbumDetail = SongAlbum.SongAlbumDetailResult;
        }
        #endregion

        #region Function
        public void SongLoadMore(bool types)
        {
            int NextPage = types ? PageIndex += 1 : PageIndex -= 1;
            if (NextPage < 0)
                return;
            if (NextPage < Total)
                Search(ShowType);
        }
        public void AddPlay(MusicSongItem input)
        {
            var SongURL = MusicFactory.Music(opt =>
            {
                opt.RequestParam = new MusicRequestInput
                {
                    Proxy = this.Proxy,
                    MusicPlatformType = this.Platform,
                    MusicType = MusicTypeEnum.PlayAddress,
                    AddressSearch = new MusicPlaySearch
                    {
                        Dynamic = input.SongId,
                        KuGouAlbumId = input.SongAlbumId,
                    }
                };
            }).Runs();
            if (SongURL.SongPlayAddressResult.CanPlay == false)
                HandyControl.Controls.MessageBox.Info("当前歌曲已下架，请切换到其他其他平台搜索");

            string CacheAddress = string.Empty;
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "Caches"));
            var SongArtist = string.Join(",", input.SongArtistName);
            var SongFile = $"{input.SongName}({input.SongAlbumName})-{SongArtist}_{Platform}.mp3";
            if (this.Platform == MusicPlatformEnum.BiliBiliMusic)
            {
                var file = SyncStatic.CreateFile(Path.Combine(dir, SongFile));
                CacheAddress = SyncStatic.WriteFile(SongURL.SongPlayAddressResult.BilibiliFileBytes, file);
            }
            else
            {
                var file = SyncStatic.CreateFile(Path.Combine(dir, SongFile));
                var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = SongURL.SongPlayAddressResult.SongURL).Build().RunBytes().FirstOrDefault();
                CacheAddress = SyncStatic.WriteFile(filebytes, file);
            }

            this.MusciService.AddPlayList(new PlayListDTO
            {
                Address = SongURL.SongPlayAddressResult.SongURL,
                SongAlbum = input.SongAlbumName,
                SongName = input.SongName,
                SongArtist = SongArtist,
                SongId = input.SongId,
                CacheAddress = CacheAddress,
                Platform = (int)this.Platform
            });
            Init();
        }
        public void Remove(Guid args)
        {
            this.MusciService.RemovePlayList(args);
            Init();
        }
        public MusicLyricResult LoadLyric(PlayListDTO args)
        {
            var data = this.MusciService.GetLyrics(args.SongId, args.Platform);
            if (data != null)
            {
                return new MusicLyricResult
                {
                    Lyrics = data.ToMapest<List<MusicLyricItemResult>>()
                };
            }
            else
            {
                var SongLyric = MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new MusicRequestInput
                    {

                        MusicPlatformType = (MusicPlatformEnum)args.Platform,
                        Proxy = this.Proxy,
                        MusicType = MusicTypeEnum.Lyric,
                        LyricSearch = new MusicLyricSearch
                        {
                            Dynamic = args.SongId
                        }
                    };
                }).Runs();
                if (SongLyric.SongLyricResult.Lyrics != null)
                {
                    var lyric = string.Join("_", SongLyric.SongLyricResult.Lyrics.Select(t => $"{t.Time}|{t.Lyric}"));
                    this.MusciService.AddLyric(args.SongId, args.Platform, lyric);
                }
                return SongLyric.SongLyricResult;
            }
        }
        public string GetPlayRoute(Guid Id)
        {
            return this.MusciService.GetPlayRoute(Id);
        }
        #endregion

        public T GetContainer<T>()
        {
            return container.Get<T>();
        }
        private void Search(int type = 1)
        {
            ShowType = type;
            switch (Platform)
            {
                case MusicPlatformEnum.QQMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.QQMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.QQMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.NeteaseMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.NeteaseMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.KuGouMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.KuGouMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.KuWoMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.KuWoMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.BiliBiliMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.BiliBiliMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                case MusicPlatformEnum.MiGuMusic:
                    {
                        if (type == 1)
                        {
                            //单曲
                            var SongItem = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                    MusicType = MusicTypeEnum.SongItem,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongItems = new ObservableCollection<MusicSongItem>(SongItem.SongItemResult.SongItems);
                            this.Platform = SongItem.SongItemResult.MusicPlatformType.Value;
                            this.Total = SongItem.SongItemResult.Total.Value;
                        }
                        else
                        {
                            //歌单
                            var SongSheet = MusicFactory.Music(opt =>
                            {
                                opt.RequestParam = new MusicRequestInput
                                {
                                    Proxy = this.Proxy,
                                    MusicPlatformType = MusicPlatformEnum.MiGuMusic,
                                    MusicType = MusicTypeEnum.SongSheet,
                                    Search = new MusicSearch
                                    {
                                        Page = PageIndex,
                                        KeyWord = KeyWord
                                    }
                                };
                            }).Runs();
                            this.SongSheets = new ObservableCollection<MusicSongSheetItem>(SongSheet.SongSheetResult.SongSheetItems);
                            this.Platform = SongSheet.SongSheetResult.MusicPlatformType.Value;
                            this.Total = SongSheet.SongSheetResult.Total.Value;
                        }
                        break;
                    }
                default:
                    throw new NullReferenceException();
            }
        }

        private void Init()
        {
            var Result = this.MusciService.GetPlayList();
            this.Count = Result.Count;
            this.PlayLists = new ObservableCollection<PlayListDTO>(Result);
        }

        protected override void OnViewLoaded()
        {
            Init();
        }
    }
}
