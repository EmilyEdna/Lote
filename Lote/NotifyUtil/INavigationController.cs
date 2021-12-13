using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.NotifyUtil
{
    public interface INavigationController
    {
        IActiveNotify Delegate { get; set; }
    }
}
