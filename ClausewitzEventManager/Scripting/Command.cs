using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausewitzEventManager.Scripting
{
    class Command
    {
        private Parser.Item item;

        public Command()
        {
        }

        public Command(Parser.Item item)
        {
            this.item = item;
        }

        internal void add(Parser.Item item)
        {
            throw new NotImplementedException();
        }
    }
}
