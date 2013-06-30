using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Microsoft.Win32;

namespace EHGrabber
{

    public partial class MainForm : Form
    {
        private int PicNumber;
        public static MainForm Me;
        private Thread m_WorkThread;
        private bool Clickable;
        private static bool BtnStatus;
        private string GalleryName;
        private string Library;
        public static int SecToSuspend=15;  // sec
        public static int FetchingInterval = 1000; // ms

        public bool AutoOpen
        {
            get { return autoOpenToolStripMenuItem.Checked; }
        }

        public bool AutoDownload
        {
            get { return automaticDownloadToolStripMenuItem.Checked; }
        }

        public MainForm()
        {
            InitializeComponent();
            Me = this;
            m_WorkThread = null;
            PicNumber = 0;
            Clickable = false;
            GalleryName = null;

            GetLibraryPath(out Library);
            if (Library == null)
                automaticDownloadToolStripMenuItem.Checked = false;
        }

        public void FlashMe()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(()=>FlashMe()));
            }
            else
                NativeMethods.FlashWindow(this.Handle, true);
        }

        private void SetLibraryPath(string Path)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "LIB", Path);
        }

        private void GetLibraryPath(out string Path)
        {
            Path=(string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "LIB", null);
        }

        public void SetGalleryTitle(string Title)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SetGalleryTitle(Title); }));
            }
            else
            {
                GalleryName = Title;
                Text = string.Format("EHGrab - {0}", Title);
            }
        }
        private void getPage(string url)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            //Request.CookieContainer.
        }

        enum URLResult
        {
            OK,
            INVALID_URL_FORMAT,
            WRONG_DOMAIN
        }

        private URLResult CheckURL(string URL)
        {
            Uri res;
            if (!(Uri.TryCreate(URLBox.Text, UriKind.Absolute, out res) && res.Scheme == Uri.UriSchemeHttp))
                return URLResult.INVALID_URL_FORMAT;

            URL = URL.ToLower();
            int HttpIndex = URL.IndexOf("http://");
            if (HttpIndex != -1)
                URL = URL.Substring(7);

            if (URL.IndexOf("g.e-hentai.org") == 0 || URL.IndexOf("exhentai.org") == 0)
                return URLResult.OK;
            else
                return URLResult.WRONG_DOMAIN;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!BtnStatus)
            {
                DbgFormFactory.GetDbgForm().Clear();
                if (CheckURL(URLBox.Text) == URLResult.OK)
                {
                    if (URLBox.Text.IndexOf("exhentai") != -1 && !LoginForm.Logined)
                    {
                        MessageBox.Show("Login is required by Exhxxtai.org!");
                        return;
                    }
                    listView1.Items.Clear();
                    GalleryName = null;
                    if (!LoginForm.Logined)
                    {
                        toolStripStatusLabel1.Text = "Stop to login";
                    }
                    m_WorkThread = new Thread(() => new FrontPage(URLBox.Text, Update_PageProgress));
                    m_WorkThread.Start();
                    button1.Text = "Stop";
                    BtnStatus = true;
                }
                else
                    MessageBox.Show("Invalid URL");
            }
            else
            {
                Abort(true);
                BtnStatus = false;
                button1.Text = "Get!";
            }
        }
        /*
        public void ResultWriteLine(string Res)
        {
            if (ResultBox.InvokeRequired)
            {
                ResultBox.Invoke(new MethodInvoker(delegate() { ResultWriteLine(Res); }));
            }
            else
            {
                ResultBox.Text += Res + "\n";
                ResultBox.Select(ResultBox.Text.Length, 0);
                ResultBox.ScrollToCaret();
            }
        }
        */

        public void SetWarningMessage(string Msg)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SetWarningMessage(Msg); }));
            }
            else
            {
                if (Msg == null)
                {
                    WarningLabel.BackColor = SystemColors.Control;
                    WarningLabel.Text = "";
                }
                else
                {
                    WarningLabel.BackColor = Color.Red;
                    WarningLabel.Text = Msg;
                }
            }
        }

        public void ResultWriteLine(string PicURL,string PageURL)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(delegate() { ResultWriteLine(PicURL,PageURL); }));
            }
            else
            {
                ListViewItem Item = listView1.Items.Add((++PicNumber).ToString());
                Item.UseItemStyleForSubItems = false;
                ListViewItem.ListViewSubItem URLItem = Item.SubItems.Add(PicURL);
                URLItem.ForeColor = Color.BlueViolet;
                URLItem = Item.SubItems.Add(PageURL);
                URLItem.ForeColor = Color.Blue;
                Item.EnsureVisible();
            }
        }

        public void Update_PageProgress(int MaxPage, int CurPage, int MaxPic, int CurPic)
        {
            if (InvokeRequired)
            {
                Invoke(new ProgCallBack(this.Update_PageProgress), MaxPage, CurPage, MaxPic, CurPic);
            }
            else
            {
                progressBar1.Maximum = MaxPage;
                progressBar1.Value = CurPage;
                progressBar2.Maximum = MaxPic;
                progressBar2.Value = CurPic;
                PageLabel.Text = "Page: " + CurPage + "/" + MaxPage;
                PicLabel.Text = "Picture: " + CurPic + "/" + MaxPic;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_WorkThread != null)
            {
                if (MessageBox.Show(this, "I'm working on you Hxxtai stuffs! Exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    m_WorkThread.Abort();
                    m_WorkThread = null;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (!DownloadForm.Clean)
            {
                e.Cancel = true;
                MessageBox.Show("Some downloaders are still alive. Close them before exiting.");
            }

        }

        public void Abort(bool Interrupt = false)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { Abort(Interrupt);}));
            }
            else
            {
                Thread TmpThread = m_WorkThread;
                m_WorkThread = null;
                BtnStatus = false;
                button1.Text = "Get!";
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                PicNumber = 0;
                PageLabel.Text = "Page:";
                PicLabel.Text = "Picture:";
                SetWarningMessage(null);
                if (!LoginForm.Logined)
                {
                    toolStripStatusLabel1.BackColor = SystemColors.Control;
                    toolStripStatusLabel1.Text = "Double click here to login";
                }
                if (AutoDownload && !Interrupt)
                    StartDownloadLVItems(Library);
                TmpThread.Abort();
            }
        }

        private void toolStripStatusLabel1_DoubleClick(object sender, EventArgs e)
        {
            if (!LoginForm.Logined)
            {
                if (m_WorkThread != null)
                    return;
                LoginForm Login = new LoginForm();
                DialogResult res = Login.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = "Logined";
                    toolStripStatusLabel1.BackColor = Color.MediumTurquoise;
                }
            }
            else
            {
                if (m_WorkThread != null)
                    return;
                if (DialogResult.Yes == MessageBox.Show(this, "Wanna logout?", "", MessageBoxButtons.YesNo))
                {
                    LoginForm.Logout();
                    LoginForm.ClearRegKey();
                    toolStripStatusLabel1.BackColor = SystemColors.Control;
                    toolStripStatusLabel1.Text = "Double click here to login";
                }
            }
        }

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo Info = listView1.HitTest(e.Location);
            int Dummy;
            if (Info.SubItem == null)
            {
                Clickable = false;
                listView1.Cursor = Cursors.Default;
                return;
            }
            if (!int.TryParse(Info.SubItem.Text, out Dummy))
            {
                Clickable = true;
                listView1.Cursor = Cursors.Hand;
            }
            else
            {
                Clickable = false;
                listView1.Cursor = Cursors.Default;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Clickable)
                        System.Diagnostics.Process.Start(listView1.HitTest(e.Location).SubItem.Text);
                    break;
                case MouseButtons.Right:
                    break;

            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }
            }

        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;
            StringBuilder Buffer = new StringBuilder();
            foreach (ListViewItem Item in listView1.Items)
            {
                Buffer.AppendLine(Item.SubItems[1].Text);
            }
            Clipboard.SetText(Buffer.ToString());
        }

        private void aToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DbgFormFactory.GetDbgForm().Show();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            MenuItem_CopySel.Visible = listView1.SelectedItems.Count != 0;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StringBuilder Buffer = new StringBuilder();
            foreach (ListViewItem Item in listView1.SelectedItems)
            {
                Buffer.AppendLine(Item.SubItems[1].Text);
            }
            Clipboard.SetText(Buffer.ToString());
        }

        public void StartDownloadLVItems(string Savepath = null)
        {
            DownloadForm Dlder = new DownloadForm(listView1.Items, Savepath, GalleryName);
            Dlder.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Nothing here");
                return;
            }
            if (GalleryName == null)
            {
                MessageBox.Show("Not prepared yet");
                return;
            }
            StartDownloadLVItems(Library);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            string U, P;
            if (LoginForm.GetRegKey(out U, out P))
            {
                if (!LoginForm.Login(U, P))
                {
                    MessageBox.Show("Your login data seems expired");
                    LoginForm.ClearRegKey();
                }
                else
                {
                    toolStripStatusLabel1.Text = "Logined";
                    toolStripStatusLabel1.BackColor = Color.MediumTurquoise;
                }
            }
        }

        private void automaticDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!automaticDownloadToolStripMenuItem.Checked)
            {
                GetLibraryPath(out Library);
                if (Library == null)
                {
                    MessageBox.Show("You need to select a default library path to use this feature.");
                    folderBrowserDialog1.ShowDialog(this);
                    if (folderBrowserDialog1.SelectedPath == "")
                        return;
                    SetLibraryPath(folderBrowserDialog1.SelectedPath + '\\');
                }
            }
            automaticDownloadToolStripMenuItem.Checked = !automaticDownloadToolStripMenuItem.Checked;
        }

        private void autoOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoOpenToolStripMenuItem.Checked = !autoOpenToolStripMenuItem.Checked;
        }

        private void URLBox_Enter(object sender, EventArgs e)
        {
            string clipboard=Clipboard.GetText();
            if (clipboard.Contains("exhentai.org") || clipboard.Contains("g.e-hentai.org"))
                URLBox.Text = clipboard;
        }

    }
}
