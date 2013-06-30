namespace EHGrabber
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.URLBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.PageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar2 = new System.Windows.Forms.ToolStripProgressBar();
            this.PicLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.WarningLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.automaticDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Number = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.URL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PageURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItem_CopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_CopySel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_DldAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItem_Dbg = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(745, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "Get!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // URLBox
            // 
            this.URLBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.URLBox.Location = new System.Drawing.Point(46, 8);
            this.URLBox.Margin = new System.Windows.Forms.Padding(2);
            this.URLBox.Name = "URLBox";
            this.URLBox.Size = new System.Drawing.Size(693, 21);
            this.URLBox.TabIndex = 2;
            this.URLBox.Enter += new System.EventHandler(this.URLBox_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "URL";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.progressBar1,
            this.PageLabel,
            this.progressBar2,
            this.PicLabel,
            this.WarningLabel,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(804, 26);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DoubleClickEnabled = true;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(128, 21);
            this.toolStripStatusLabel1.Text = "Double click to login";
            this.toolStripStatusLabel1.DoubleClick += new System.EventHandler(this.toolStripStatusLabel1_DoubleClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(75, 20);
            // 
            // PageLabel
            // 
            this.PageLabel.Name = "PageLabel";
            this.PageLabel.Size = new System.Drawing.Size(80, 21);
            this.PageLabel.Text = "Page:          ";
            // 
            // progressBar2
            // 
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(75, 20);
            // 
            // PicLabel
            // 
            this.PicLabel.Name = "PicLabel";
            this.PicLabel.Size = new System.Drawing.Size(90, 21);
            this.PicLabel.Text = "Picture:          ";
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = false;
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(222, 21);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automaticDownloadToolStripMenuItem,
            this.autoOpenToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(67, 24);
            this.toolStripDropDownButton1.Text = "Options";
            // 
            // automaticDownloadToolStripMenuItem
            // 
            this.automaticDownloadToolStripMenuItem.Checked = true;
            this.automaticDownloadToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.automaticDownloadToolStripMenuItem.Name = "automaticDownloadToolStripMenuItem";
            this.automaticDownloadToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.automaticDownloadToolStripMenuItem.Text = "Auto Download";
            this.automaticDownloadToolStripMenuItem.Click += new System.EventHandler(this.automaticDownloadToolStripMenuItem_Click);
            // 
            // autoOpenToolStripMenuItem
            // 
            this.autoOpenToolStripMenuItem.Name = "autoOpenToolStripMenuItem";
            this.autoOpenToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.autoOpenToolStripMenuItem.Text = "Auto Open";
            this.autoOpenToolStripMenuItem.Click += new System.EventHandler(this.autoOpenToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Number,
            this.URL,
            this.PageURL});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 33);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(805, 364);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            // 
            // Number
            // 
            this.Number.Text = "No";
            this.Number.Width = 43;
            // 
            // URL
            // 
            this.URL.Text = "URL";
            this.URL.Width = 615;
            // 
            // PageURL
            // 
            this.PageURL.Text = "PageURL";
            this.PageURL.Width = 296;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_CopyAll,
            this.MenuItem_CopySel,
            this.toolStripSeparator1,
            this.MenuItem_DldAll,
            this.toolStripSeparator2,
            this.MenuItem_Dbg});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 112);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // MenuItem_CopyAll
            // 
            this.MenuItem_CopyAll.Name = "MenuItem_CopyAll";
            this.MenuItem_CopyAll.Size = new System.Drawing.Size(182, 24);
            this.MenuItem_CopyAll.Text = "Copy All";
            this.MenuItem_CopyAll.Click += new System.EventHandler(this.aToolStripMenuItem_Click);
            // 
            // MenuItem_CopySel
            // 
            this.MenuItem_CopySel.Name = "MenuItem_CopySel";
            this.MenuItem_CopySel.Size = new System.Drawing.Size(182, 24);
            this.MenuItem_CopySel.Text = "Copy Selected";
            this.MenuItem_CopySel.Visible = false;
            this.MenuItem_CopySel.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // MenuItem_DldAll
            // 
            this.MenuItem_DldAll.Name = "MenuItem_DldAll";
            this.MenuItem_DldAll.Size = new System.Drawing.Size(182, 24);
            this.MenuItem_DldAll.Text = "Download All";
            this.MenuItem_DldAll.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // MenuItem_Dbg
            // 
            this.MenuItem_Dbg.Name = "MenuItem_Dbg";
            this.MenuItem_Dbg.Size = new System.Drawing.Size(182, 24);
            this.MenuItem_Dbg.Text = "Debug Window";
            this.MenuItem_Dbg.Click += new System.EventHandler(this.aToolStripMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 425);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.URLBox);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(820, 160);
            this.Name = "MainForm";
            this.Text = "EHGrabber";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox URLBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Number;
        private System.Windows.Forms.ColumnHeader URL;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.ToolStripStatusLabel PageLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar2;
        private System.Windows.Forms.ToolStripStatusLabel PicLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopyAll;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Dbg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_CopySel;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_DldAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ColumnHeader PageURL;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem automaticDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel WarningLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem autoOpenToolStripMenuItem;
    }
}

