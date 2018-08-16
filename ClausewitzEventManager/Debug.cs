using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausewitzEventManager
{
    static class Debug
    {

        static public void Log(string s)
        {
            MainForm.instance.AddToLog(s);
        }
    }
}
