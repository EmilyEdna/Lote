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
        OptionRootDTO AddOrUpdate(OptionRootDTO input);
        OptionRootDTO Get();
    }
    public class OptionService : Lite, IOptionService
    {
        public OptionRootDTO AddOrUpdate(OptionRootDTO input)
        {
            var Global = input.ToMapest<GlobalSetting>();
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

        public OptionRootDTO Get()
        {
            return LiteBase().Queryable<GlobalSetting>().First().ToMapest<OptionRootDTO>();
        }
    }
}
