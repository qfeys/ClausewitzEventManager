﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClausewitzEventManager
{
    public partial class MainForm : Form
    {
        public static MainForm instance;

        public MainForm()
        {
            InitializeComponent();
            console.Lines = new string[100];
            if (instance == null)
                instance = this;
            Shown += MainForm_Shown;
        }

        private void MainForm_Shown(Object sender, EventArgs e)
        {
            this.console.Text = "test: " + DirectoryManager.GetDir();
            DirectoryManager.FindCK2Folder();
            //Parser.Parse(@"C:\Program Files (x86)\Paradox Interactive\Crusader Kings II Jade Dragon\common\traits\01_traits.txt");
            //Parser.Parse(@"C:\Program Files (x86)\Paradox Interactive\Crusader Kings II Jade Dragon\common\region_colors.txt");
            List<Parser.Item> coreEvents = Parser.ParseEvents(DirectoryManager.GetDir());
            Data.SetCoreEvents(coreEvents);
            PopulateTree();
        }

        private void PopulateTree()
        {
            TreeNode core = new TreeNode("core");
            foreach (Data.EventList file in Data.CoreEvents)
            {
                TreeNode fileNode = core.Nodes.Add(file.Name);
                foreach(CW_Event ev in file.List)
                {
                    fileNode.Nodes.Add(ev.ToString());
                }
            }
            treeView1.Nodes.Add(core);
        }

        internal void AddToLog(string s)
        {
            console.AppendText(Environment.NewLine + s);
            console.ScrollToCaret();
            //this.console.Text = s + "\n" + this.console.Text;
        }
    }
}
