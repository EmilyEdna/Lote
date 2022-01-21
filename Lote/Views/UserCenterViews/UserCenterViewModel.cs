using Lote.CommonWindow;
using Lote.CommonWindow.ViewMdeol;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
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
            NovelPageIndex = 1;
        }
        private ObservableCollection<LoteNovelHistoryDTO> _NovelHistories;
        public ObservableCollection<LoteNovelHistoryDTO> NovelHistories
        {
            get { return _NovelHistories; }
            set { SetAndNotify(ref _NovelHistories, value); }
        }

        public int NovelPageIndex { get; set; }

        public void Init()
        {
            NovelHistories = new ObservableCollection<LoteNovelHistoryDTO>(container.Get<IHistoryService>().GetNovelHistory(NovelPageIndex));
        }

        protected override void OnViewLoaded()
        {
            Init();
        }

        public void Reader(LoteNovelHistoryDTO args)
        {
            var vm = container.Get<NovelContentWindowsViewModel>();
            vm.NovelContent = args.ToMapest<NovelContentResult>();
            vm.BookName = args.BookName;

            NovelContentWindows win = null;
            if (BootResource.NovelContentWindow.ContainsKey(nameof(NovelContentWindows)))
            {
                win = BootResource.NovelContentWindow[nameof(NovelContentWindows)];
                win.Close();
                BootResource.NovelContentWindow.Clear();
                win = new NovelContentWindows();
                win.DataContext = vm;
                BootResource.NovelContentWindow[nameof(NovelContentWindows)] = win;
                win.Show();
            }
            else
            {
                win = new NovelContentWindows();
                win.DataContext = vm;
                BootResource.NovelContentWindow[nameof(NovelContentWindows)] = win;
                win.Show();
            }
        }
    }
}
