using System;
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
            if (instance == null)
                instance = this;

            this.label1.Text = "test: " + DirectoryManager.GetDir();
            DirectoryManager.FindCK2Folder();
        }

        internal void AddToLog(string s)
        {
            this.label1.Text = s + "\n" + this.label1.Text;
        }
    }
}
