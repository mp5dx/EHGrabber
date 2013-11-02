using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
//using System.Threading.Tasks;
using System.Threading;

namespace EHGrabber
{


    public partial class DownloadForm : Form
    {
        private int TaskIndex;
        private WebClient Downloader;
        private string Gallery;
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

        public string GalleryName
        {
            get { return Gallery; }
            set
            {
                Gallery = value.Replace('\\', ' ')
                              .Replace('/', ' ').Replace(':', ' ')
                              .Replace('*', ' ').Replace('?', ' ')
                              .Replace('"', ' ').Replace('<', ' ')
                              .Replace('>', ' ').Replace('|', ' ');
                StorePath += GalleryName + '\\';
                Directory.CreateDirectory(StorePath);
            }
        }

        private string ResolveNewPicURL(string PageURL)
        {
            Random Rand_nl = new Random();
            string NewPage = PageURL.TrimEnd('/') + "?nl=" + (Rand_nl.Next(1000) + 1);
            PicPage Secondary = new PicPage(NewPage, true);
            return Secondary.GetPicURL();
        }

        private void OnComplete(object sender, AsyncCompletedEventArgs e,WorkerContext ctx)
        {
            Lock.Release();
            if (e.Cancelled)
                return;
            if (e.Error != null)
            {
                //listView1.Items[TaskIndex++].SubItems[3].Text = "Error";
                ctx.URL = ResolveNewPicURL(ctx.PageURL);
                ThreadPool.QueueUserWorkItem(ThreadPoolWorker, ctx);
            }
            else
            {
                Complete++;
                SetLVItemStatus(ctx.Index, "Done");
                
                //EventArr[index % 5].Set();
                if (Complete == listView1.Items.Count && Exitable)
                {
                    if (MainForm.Me.AutoOpen)
                        System.Diagnostics.Process.Start(StorePath);
                    Invoke(new MethodInvoker(delegate { Close(); })); 
                }
            }
        }

        private void OnProgChanged(object sender, DownloadProgressChangedEventArgs e,WorkerContext ctx)
        {
            SetLVItemStatus(ctx.Index, string.Format("{0}%", e.ProgressPercentage));
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

        Semaphore Lock;
        int Complete;

        private class WorkerContext
        {
            public string URL;
            public string Filename;
            public string PageURL;
            public int Index;

            public WorkerContext(int index, string url,string pageurl, string filename)
            {
                URL = url;
                Filename = filename;
                PageURL = pageurl;
                Index = index;
            }
        }

        private void ThreadPoolWorker(object Ctx)
        {
            WorkerContext Context = (WorkerContext)Ctx;
            WebClient Downloader = new WebClient();
            Downloader.DownloadProgressChanged += (sender,e)=>OnProgChanged(sender,e,Context);
            Downloader.DownloadFileCompleted += (sender, e) => OnComplete(sender, e, Context);
            Lock.WaitOne();
            Downloader.DownloadFileAsync(new Uri(Context.URL), StorePath + Context.Filename);
            SetLVItemStatus( Context.Index,"0%");
        }
        /*
        private void WorkerThread()
        {
            for(int i=0;i<listView1.Items.Count;++i)
            {
                EventArr.Add(new ManualResetEvent(false));
                ThreadPool.QueueUserWorkItem(ThreadPoolWorker, i);
                if (EventArr.Count == 5 || i==listView1.Items.Count-1)
                {
                    WaitHandle.WaitAll(EventArr.ToArray());
                    EventArr.Clear();
                }
            }
            if (MainForm.Me.AutoOpen)
                System.Diagnostics.Process.Start(StorePath);
            Close();
        }
        */
        public DownloadForm(string SavePath = null)
        {
            InitializeComponent();

            ThreadPool.SetMaxThreads(8,8);
            Lock = new Semaphore(5, 5);
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

            Complete = 0;

        }


        public void SetLVItemStatus(int index, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SetLVItemStatus(index, status); }));
            }
            else
            {
                listView1.Items[index].SubItems[3].Text = status;
            }
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
                ThreadPool.QueueUserWorkItem(ThreadPoolWorker, new WorkerContext(ListItem.Index,PicURL,PageURL,FileName));
            }
        }



        private void CleanUp()
        {
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
            if (Complete!=listView1.Items.Count||!Exitable)
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
