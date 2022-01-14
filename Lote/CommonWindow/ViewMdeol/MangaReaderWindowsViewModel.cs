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

        private ObservableCollection<string> _ImageURL;
        public ObservableCollection<string> ImageURL
        {
            get { return _ImageURL; }
            set { SetAndNotify(ref _ImageURL, value); }
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
        #endregion

        public void InitCurrent()
        {
            var MangaContent = MangaFactory.Manga(opt =>
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
            ImageURL = new ObservableCollection<string>(MangaContent.ContentResults.ImageURL);
        }
    }
}
