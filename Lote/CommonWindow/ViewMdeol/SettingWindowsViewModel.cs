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


namespace Lote.CommonWindow.ViewMdeol
{
    public class SettingWindowsViewModel:Screen
    {
        private readonly IContainer container;
        public IOptionService optionService;
        public SettingWindowsViewModel(IContainer container)
        {
            this.container = container;
            Root = container.Get<IOptionService>().Get() ?? new LoteSettingDTO();
            optionService = container.Get<IOptionService>();
        }

        #region Property
        private LoteSettingDTO _Root;
        public LoteSettingDTO Root
        {
            get { return _Root; }
            set { SetAndNotify(ref _Root, value); }
        }
        #endregion
    }
}
