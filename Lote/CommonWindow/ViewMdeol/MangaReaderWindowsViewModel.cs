using Manga.SDK;
using Manga.SDK.ViewModel;
using Manga.SDK.ViewModel.Emums;
using Manga.SDK.ViewModel.Response;
using Stylet;
using StyletIoC;
using Manga.SDK.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lote.Core.Service.DTO;
using Lote.Core.Service;
using XExten.Advance.LinqFramework;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.StaticFramework;
using System.IO;
using System.Windows.Media.Imaging;
using Lote.Common;
using System.Collections;

namespace Lote.CommonWindow.ViewMdeol
{
    public class MangaReaderWindowsViewModel : Screen
    {
        private readonly IContainer container;
        private readonly OptionRootDTO root;
        private readonly MangaProxy Proxy;
        public MangaReaderWindowsViewModel(IContainer container)
        {
            this.container = container;
            this.root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();
            this.Proxy = new MangaProxy
            {
                IP = root.ProxyIP.IsNullOrEmpty() ? String.Empty : root.ProxyIP,
                PassWord = root.ProxyPwd.IsNullOrEmpty() ? String.Empty : root.ProxyPwd,
                Port = Convert.ToInt32(root.ProxyPort.IsNullOrEmpty() ? "-1" : root.ProxyPort),
                UserName = root.ProxyAccount.IsNullOrEmpty() ? String.Empty : root.ProxyAccount
            };
        }

        #region Property
        private ObservableCollection<MangaChapterResult> _Chapters;
        public ObservableCollection<MangaChapterResult> Chapters
        {
            get { return _Chapters; }
            set { SetAndNotify(ref _Chapters, value); }
        }

        private bool _Loading;
        public bool Loading
        {
            get { return _Loading; }
            set { SetAndNotify(ref _Loading, value); }
        }

        private ObservableCollection<BitmapSource> _Bit;
        public ObservableCollection<BitmapSource> Bit
        {
            get { return _Bit; }
            set { SetAndNotify(ref _Bit, value); }
        }
        private int _Index;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { SetAndNotify(ref _Index, value); }
        }

        public ArrayList Names { get; set; }
        #endregion

        public async Task InitCurrent()
        {
            SystemHelper.SystemGC();

             var MangaContent = await Task.Factory.StartNew(() =>
            {
                return MangaFactory.Manga(opt =>
                 {
                     opt.RequestParam = new MangaRequestInput
                     {
                         MangaType = MangaEnum.Content,
                         Proxy = this.Proxy,
                         Content = new MangaContent
                         {
                             Address = Chapters[Index].Address
                         }
                     };
                 }).Runs();

            });

            await CacheLocal(MangaContent.ContentResults.ImageURL, Chapters[Index].TagKey);
            Loading = false;
            SystemHelper.SystemGC();
        }

        protected async Task CacheLocal(List<string> URL, string key)
        {
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteResource", "MangaCaches", key));

            var all = Directory.GetFiles(dir).OrderBy(t => t).ToList();

            if (all.Count() == URL.Count)
            {
                Bit = new ObservableCollection<BitmapSource>();
                Names = new ArrayList();
                for (int index = 0; index < all.Count(); index++)
                {
                    FileStream fileStream = new FileStream(all[index], FileMode.Open, FileAccess.Read);
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    Names.Add(all[index]);
                    Bit.Add(ImageHelper.BitmapToBitmapImage(array));
                    fileStream.Close();
                }
            }
            else
            {
                SyncStatic.DeleteFolder(dir);

                var dirs = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "LoteResource", "MangaCaches", key));

                var Node = IHttpMultiClient.HttpMulti.AddNode(opt =>
                {
                    opt.NodePath = URL.FirstOrDefault();
                });

                for (int index = 1; index < URL.Count; index++)
                {
                    Node = Node.AddNode(opt =>
                    {
                        opt.NodePath = URL[index];
                    });
                }
                var data = await Node.Build().RunBytesAsync();

                Bit = new ObservableCollection<BitmapSource>();
                Names = new ArrayList();
                data.ForEach(item =>
                {
                    var file = SyncStatic.CreateFile(Path.Combine(dirs, DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                    SyncStatic.WriteFile(item, file);
                    Names.Add(file);
                    Bit.Add(ImageHelper.BitmapToBitmapImage(item));
                });
            }
        }
    }
}
