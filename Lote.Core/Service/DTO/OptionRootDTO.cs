using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lote.Core.Service.DTO
{
    public class OptionRootDTO
    {
        #region Proxy
        public Guid? Id { get; set; }
        public string ProxyIP { get; set; }
        public int? ProxyPort { get; set; }
        public string ProxyAccount { get; set; }
        public string ProxyPwd { get; set; }
        public int? CacheSpan { get; set; }
        #endregion

        #region Wk
        public string WkAccount { get; set; }
        public string WkPwd { get; set; }
        #endregion
    }
}
