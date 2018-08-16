﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClausewitzEventManager
{
    static class DirectoryManager
    {
        static string ck2dir;

        static void FindCK2Folder()
        {
            string start = @"C:\Program Files (x86)\Paradox Interactive";
            if (Directory.Exists(start))
            {
                foreach( string dir in Directory.GetDirectories(start))
                {
                    string sub = dir.Substring(0, Math.Min(dir.Length, 8));
                    if (sub == "Crusader")
                    {
                        ck2dir = start + @"\" + sub;
                        return;
                    }
                }
            }
        }

    }
}