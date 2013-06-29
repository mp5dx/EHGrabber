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
        private string StorePath;
        private bool Unexitable;
        private bool WriteFlag;
        private static int Count;
        private bool WannaExit;
        private AutoResetEvent CleanUpAlerter;
        //private bool OpenAfterFinish;

        public static bool Clean
        {
            get { return Count == 0; }
        }

        private void CleanUpDoneTasks()
        {
            TaskIndex = 0;
            WriteFlag = false;
            Unexitable = false;
            foreach (ListViewItem Item in listView1.Items)
                if (Item.SubItems[3].Text == "Done")
                    listView1.Items.Remove(Item);
        }

        private string ResolveNewPicURL(string PageURL)
        {
            Random Rand_nl=new Random();
            string NewPage = PageURL.TrimEnd('/') + "?nl=" + (Rand_nl.Next(1000) + 1);
            PicPage Secondary = new PicPage(NewPage, true);
            return Secondary.GetPicURL();
        }

        private void ProceedIfAble()
        {
            if (TaskIndex < listView1.Items.Count)
                StartAsyncDownload();
            else if (!Unexitable) //All finished
            {
                if (MainForm.Me.AutoOpen)
                    System.Diagnostics.Process.Start(StorePath);
                Close();
            }
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
            }
        }

        private void OnComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (WannaExit)
                return;
            else
            {
                if (e.Error != null)
                {
                    listView1.Items[TaskIndex++].SubItems[3].Text = "Error";
                    Unexitable = true;
                }
                else
                    listView1.Items[TaskIndex++].SubItems[3].Text = "Done";
            }
            timer1.Enabled = false;
            ProceedIfAble();
        }

        private void OnProgChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            listView1.Items[TaskIndex].SubItems[3].Text = string.Format("{0}%", e.ProgressPercentage);
            WriteFlag = true;
        }

        private void StartAsyncDownload()
        {
            Downloader.DownloadFileAsync(new Uri(listView1.Items[TaskIndex].SubItems[1].Text), StorePath + listView1.Items[TaskIndex].SubItems[0].Text);
            listView1.Items[TaskIndex].SubItems[3].Text = "0%";
            timer1.Enabled = true;
        }

        private void NormalizePath(ref string Path)
        {
            if (Path.Length == 0)
                return;
            if (Path[Path.Length - 1] != '\\')
                Path += '\\';
        }

        public DownloadForm(ListView.ListViewItemCollection List,string SavePath=null,string GalleryName=null)
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

            if (GalleryName != null)
            {
                GalleryName = GalleryName.Replace('\\', ' ')
                    .Replace('/', ' ').Replace(':', ' ')
                    .Replace('*', ' ').Replace('?', ' ')
                    .Replace('"', ' ').Replace('<', ' ')
                    .Replace('>', ' ').Replace('|', ' ');
                StorePath += GalleryName +'\\';
            }
            Directory.CreateDirectory(StorePath);

            Downloader = new WebClient();
            Downloader.DownloadFileCompleted += new AsyncCompletedEventHandler(OnComplete);
            Downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnProgChanged);

            foreach (ListViewItem AddItem in List)
            {
                string URL = AddItem.SubItems[1].Text;
                string FileName = URL.Substring(URL.LastIndexOfAny("/=?".ToCharArray()) + 1);
                ListViewItem ListItem = listView1.Items.Add(FileName);
                ListItem.SubItems.Add(URL);
                ListItem.SubItems.Add(AddItem.SubItems[2]);
                ListItem.SubItems.Add("Waiting");
                
            }
            listView1.Items[TaskIndex] = listView1.Items[0];
            StartAsyncDownload();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (WriteFlag)
            {
                WriteFlag = false;
            }
            else
            {
                Downloader.CancelAsync();
                Unexitable = true;
                timer1.Enabled = false;
                //listView1.Items[TaskIndex++].SubItems[3].Text = "Too Slow";
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
            if (TaskIndex < listView1.Items.Count)
            {
                if (DialogResult.No == MessageBox.Show(this, "Haven't finished downloading. Really wanna exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                { 
                    e.Cancel = true;
                    return;
                }
            }
            CleanUp();
        }
    }
}
