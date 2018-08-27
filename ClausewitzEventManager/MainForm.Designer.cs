﻿namespace ClausewitzEventManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.console = new System.Windows.Forms.TextBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // console
            // 
            this.console.BackColor = System.Drawing.SystemColors.ControlDark;
            this.console.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.console.Location = new System.Drawing.Point(12, 348);
            this.console.MaximumSize = new System.Drawing.Size(800, 85);
            this.console.MinimumSize = new System.Drawing.Size(800, 85);
            this.console.Multiline = true;
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.console.Size = new System.Drawing.Size(800, 85);
            this.console.TabIndex = 0;
            this.console.Text = "CONSOLE";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 25);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(243, 317);
            this.treeView1.TabIndex = 1;
            // 
            // treeTitle
            // 
            this.treeTitle.AccessibleName = "TreeTitle";
            this.treeTitle.AutoSize = true;
            this.treeTitle.Location = new System.Drawing.Point(12, 9);
            this.treeTitle.Name = "treeTitle";
            this.treeTitle.Size = new System.Drawing.Size(35, 13);
            this.treeTitle.TabIndex = 2;
            this.treeTitle.Text = "Events";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(826, 445);
            this.Controls.Add(this.treeTitle);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.console);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox console;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label treeTitle;
    }
}

