using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace EHGrabber
{


    public partial class DownloadForm : Form
    {
        private int TaskIndex;
        private WebClient Downloader;
        public string GalleryName;
        private bool Initialized;
        private string StorePath;
        private bool Exitable;
        private static int Count;
        private bool WannaExit;
        private AutoResetEvent CleanUpAlerter;
        //private bool OpenAfterFinish;

        public static bool Clean
        {
            get { return Count == 0; }
        }

        public bool Done
        {
            set { Exitable = value; }
        }

        private string ResolveNewPicURL(string PageURL)
        {
            Random Rand_nl = new Random();
            string NewPage = PageURL.TrimEnd('/') + "?nl=" + (Rand_nl.Next(1000) + 1);
            PicPage Secondary = new PicPage(NewPage, true);
            return Secondary.GetPicURL();
        }

        private void ProceedIfAble()
        {
            if (listView1.Items.Count == TaskIndex || GalleryName == null)
            {
                if (Exitable) //All finished
                {
                    if (MainForm.Me.AutoOpen)
                        System.Diagnostics.Process.Start(StorePath);
                    Close();
                }
                timer1.Enabled = true;
                return;
            }
            if (TaskIndex < listView1.Items.Count)
                StartAsyncDownload();
            /*
            else //If there is still something we can download
            {
                Text = "Downloader - Finding secondary server for slow files";
                CleanUpDoneTasks();
                foreach (ListViewItem SlowShits in listView1.Items)
                {
                    SlowShits.SubItems[1].Text = ResolveNewPicURL(SlowShits.SubItems[2].Text);
                    SlowShits.SubItems[3].Text = "Waiting";
                }
                ProceedIfAble();
            }*/
        }

        private void OnComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (WannaExit)
                return;
            else
            {
                do
                {
                    if (e.Cancelled)
                        break;
                    if (e.Error != null)
                    {
                        //listView1.Items[TaskIndex++].SubItems[3].Text = "Error";
                        listView1.Items[TaskIndex].SubItems[1].Text = ResolveNewPicURL(listView1.Items[TaskIndex].SubItems[2].Text);
                    }
                    else
                        listView1.Items[TaskIndex++].SubItems[3].Text = "Done";
                } while (false);
            }
            timer1.Enabled = false;
            timer2.Enabled = false;
            ProceedIfAble();
        }

        private void OnProgChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            listView1.Items[TaskIndex].SubItems[3].Text = string.Format("{0}%", e.ProgressPercentage);
            timer2.Enabled = false;
            timer2.Enabled = true;
        }

        private void StartAsyncDownload()
        {
            timer1.Enabled = false;
            Downloader.DownloadFileAsync(new Uri(listView1.Items[TaskIndex].SubItems[1].Text), StorePath + listView1.Items[TaskIndex].SubItems[0].Text);
            listView1.Items[TaskIndex].SubItems[3].Text = "0%";
            timer2.Enabled = true;
        }

        private void NormalizePath(ref string Path)
        {
            if (Path.Length == 0)
                return;
            if (Path[Path.Length - 1] != '\\')
                Path += '\\';
        }

        public DownloadForm(string SavePath = null)
        {
            InitializeComponent();

            CleanUpAlerter = new AutoResetEvent(false);
            TaskIndex = 0;
            Count++;
            if (SavePath == null)
            {
                folderBrowserDialog1.ShowDialog();
                SavePath = folderBrowserDialog1.SelectedPath;
            }
            NormalizePath(ref SavePath);

            StorePath = SavePath;

            Downloader = new WebClient();
            Downloader.DownloadFileCompleted += new AsyncCompletedEventHandler(OnComplete);
            Downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnProgChanged);
            //listView1.Items[TaskIndex] = listView1.Items[0];
            //StartAsyncDownload();
            ProceedIfAble();
        }

        public void AddFile(string PageURL, string PicURL)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AddFile(PageURL, PicURL); }));
            }
            else
            {
                string FileName = PicURL.Substring(PicURL.LastIndexOfAny("/=?".ToCharArray()) + 1);
                ListViewItem ListItem = listView1.Items.Add(FileName);
                ListItem.SubItems.Add(PicURL);
                ListItem.SubItems.Add(PageURL);
                ListItem.SubItems.Add("Waiting");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listView1.Items.Count != TaskIndex)
            {
                timer1.Enabled = false;
                if (GalleryName != null && !Initialized)
                {
                    GalleryName = GalleryName.Replace('\\', ' ')
                        .Replace('/', ' ').Replace(':', ' ')
                        .Replace('*', ' ').Replace('?', ' ')
                        .Replace('"', ' ').Replace('<', ' ')
                        .Replace('>', ' ').Replace('|', ' ');
                    StorePath += GalleryName + '\\';
                    Directory.CreateDirectory(StorePath);
                    Initialized = true;
                }
                ProceedIfAble();
            }
        }

        private void CleanUp()
        {
            Downloader.CancelAsync();
            WannaExit = true;
            //CleanUpAlerter.WaitOne();
            Count--;
        }

        private void forceExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TaskIndex < listView1.Items.Count||!Exitable)
            {
                if (DialogResult.No == MessageBox.Show(this, "Haven't finished downloading. Really wanna exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }
            }
            CleanUp();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Downloader.CancelAsync();
            listView1.Items[TaskIndex].SubItems[1].Text = ResolveNewPicURL(listView1.Items[TaskIndex].SubItems[2].Text);
            timer2.Enabled = false;
        }
    }

    public class DownloadContext
    {
        public string m_URL;
        public DownloadForm m_Downloader;
        public ProgCallBack m_Notify;

        public DownloadContext(string URL, DownloadForm Downloader, ProgCallBack Notify)
        {
            m_URL = URL;
            m_Downloader = Downloader;
            m_Notify = Notify;
        }
    };
}
