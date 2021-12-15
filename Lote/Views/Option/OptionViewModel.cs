using HandyControl.Controls;
using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.InternalFramework.Securities.Common;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace Lote.Views.Option
{
    public class OptionViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IWindowManager windowManager;
        public OptionViewModel(IContainer container, IWindowManager windowManager)
        {
            this.container = container;
            this.windowManager = windowManager;
        }

        #region Property
        private OptionRootDTO _Root;
        public OptionRootDTO Root
        {
            get { return _Root; }
            set { SetAndNotify(ref _Root, value); }
        }
        #endregion

        #region Method
        protected override void OnViewLoaded()
        {
            Root = container.Get<IOptionService>().Get() ?? new OptionRootDTO();

            base.OnViewLoaded();
        }

        public void Submit()
        {
            Root = container.Get<IOptionService>().AddOrUpdate(Root);
            MessageBox.Info("操作成功", "提示");
        }
        #endregion
    }
}
