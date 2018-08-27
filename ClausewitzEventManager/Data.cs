using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausewitzEventManager
{
    static class Data
    {
        static public List<EventList> CoreEvents { get; private set; }

        static public void SetCoreEvents(List<Parser.Item> items)
        {
            CoreEvents = new List<EventList>();
            foreach (var it in items)
                if (it != null)
                    CoreEvents.Add(new EventList(it));
        }

        public class EventList
        {
            public string Name { get; private set; }
            public List<CW_Event> List { get; private set; }

            public EventList(Parser.Item root)
            {
                Name = root.name;
                List = new List<CW_Event>();
                foreach (Parser.Item item in root.GetChilderen())
                {
                    if (item.name == "namespace")
                        //item.GetChilderen().ForEach(ch => List.Add(CW_Event.FromParsedItem(ch)));
                        continue;
                    else
                        List.Add(CW_Event.FromParsedItem(item));
                }
            }
        }

    }
}
