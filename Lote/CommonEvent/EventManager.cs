using Lote.Core.Service;
using Lote.Core.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.EventFramework.EventSources;
using XExten.Advance.EventFramework.SubscriptEvent;
using XExten.Advance.LinqFramework;

namespace Lote.CommonEvent
{
    public class EventManager : IEventSubscriber
    {
        private IHistoryService Service;
        public EventManager()
        {
            Service = new HistoryService();
        }

        [EventSubscribe("AddNovelHistory")]
        public Task AddNovelHistory(IEventSource source)
        {
            Service.AddNovelHistory(source.Payload.ToMapest<LoteNovelHistoryDTO>());
            return Task.CompletedTask;
        }
    }
}
