using Lote.Core.Service;
using Lote.Core.Service.DTO;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Views.Option
{
    public class OptionViewModel : Screen
    {
        private readonly IContainer container;
        public OptionViewModel(IContainer container)
        {
            this.container = container;
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
            Root = container.Get<IOptionService>().Get();

            base.OnViewLoaded();
        }
        #endregion
    }
}
