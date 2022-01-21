using Lote.Core.Service.DTO;
using XExten.Advance.LinqFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lote.Core.Model;

namespace Lote.Core.Service
{
    public interface IOptionService
    {
        LoteSettingDTO AddOrUpdate(LoteSettingDTO input);
        LoteSettingDTO Get();
    }
    public class OptionService : Lite, IOptionService
    {
        public LoteSettingDTO AddOrUpdate(LoteSettingDTO input)
        {
            var Global = input.ToMapest<LoteSetting>();
            if (input.Id.HasValue)
            {
                LiteBase().Updateable(Global).ExecuteCommandHasChange();
            }
            else
            {
                var IGlobal = LiteBase().Insertable(Global).CallEntityMethod(t => t.Create()).ExecuteReturnEntity();
                input.Id = IGlobal.Id;
            }
            return input;
        }

        public LoteSettingDTO Get()
        {
            return LiteBase().Queryable<LoteSetting>().First().ToMapest<LoteSettingDTO>();
        }
    }
}
