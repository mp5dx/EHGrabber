using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using System.Text.RegularExpressions;
using System.Threading;

namespace EHGrabber
{

    public delegate void ProgCallBack(int MaxPage,int CurPage,int MaxPic,int CurPic,DownloadContext Context);

    public class WebPage
    {
        protected string m_URL; // Should not end with '/'
        protected string m_source;

        public void ShowSource(string Source)
        {
            DbgForm SourceWindow = new DbgForm(Source);
            SourceWindow.Show();
        }

        public string Source
        {
            get { return m_source; }
        }

        protected void FetchSource()
        {
            WebRequest ReqFrontPage = HttpWebRequest.Create(m_URL);
            ReqFrontPage.Method = "GET";
            CookieContainer Cookie=new CookieContainer();

            ((HttpWebRequest)ReqFrontPage).CookieContainer = Cookie;
            if (LoginForm.ID != null)
            {
                Cookie.Add(new Uri("http://exhentai.org"), new Cookie("ipb_member_id",LoginForm.ID));
                Cookie.Add(new Uri("http://exhentai.org"), new Cookie("ipb_pass_hash", LoginForm.Password));
            }
            StreamReader Reader = new StreamReader(ReqFrontPage.GetResponse().GetResponseStream());
            m_source = Reader.ReadToEnd();
        }

        public WebPage(string URL)
        {
            m_URL = URL;
            FetchSource();
            if (m_source.IndexOf("Your IP address has been temporarily banned") != -1)
            {
                MessageBox.Show("You are banned!");
                MainForm.Me.Abort(true);
            }
            else if (m_source.IndexOf("too fast") != -1)
            {
                MainForm.Me.FlashMe();
                MainForm.FetchingInterval += 200;
                for (int Sec = MainForm.SecToSuspend; Sec > 0; --Sec)
                {
                    MainForm.Me.SetWarningMessage(string.Format("Too fast. Suspending: {0}s",Sec));
                    Thread.Sleep(1000);
                }
                MainForm.Me.SetWarningMessage(null);
                WebPage TmpPage = new WebPage(URL);
                m_source = TmpPage.Source;
            }
            else if (m_source.IndexOf("for the fjords") != -1)
            {
                MessageBox.Show("This gallery is no longer avaliable to kids!");
                MainForm.Me.Abort(true);
            }
        }
    }

    public class PicPage : WebPage
    {
        public string m_PicURL;

        private string PostProcessing(ref string URL)
        {
            URL=URL.Replace("&amp;", "&");
            return URL;
        }

        public string GetPicURL()
        {
            if (m_URL.IndexOf("exhentai") != -1)
            {
                Match Label_i3 = Regex.Match(m_source, @"<div id=""i3"">");

                Match PicURL = Regex.Match(m_source.Substring(Label_i3.Index), "<img.*?>");
                PicURL = Regex.Match(PicURL.Value, "src=.*?>");
                string Raw = PicURL.Value.Substring(5);
                Raw = Raw.Substring(0, Raw.IndexOf("\""));
                return PostProcessing(ref Raw);
            }
            else
            {
                MatchCollection Label_iframe = Regex.Matches(m_source, "iframe");

                Match PicURL = Regex.Match(m_source.Substring(Label_iframe[1].Index), "<img.*?>");
                PicURL = Regex.Match(PicURL.Value, "src=.*?>");
                string Raw = PicURL.Value.Substring(5);
                Raw = Raw.Substring(0, Raw.IndexOf("\""));
                return PostProcessing(ref Raw);
            }
        }

        public PicPage(string URL,bool PlainMode=false)
            : base(URL)
        {
            if (PlainMode)
                return;
            m_PicURL = GetPicURL();
            MainForm.Me.ResultWriteLine(m_PicURL,m_URL);
            Thread.Sleep(MainForm.FetchingInterval);
        }
    }

    public class GalleryPage : WebPage
    {
        private List<string> m_PicList;
        private void FetchPicList()
        {
            MatchCollection Label_gdtm = Regex.Matches(m_source, "gdtm");
            foreach (Match gdtm in Label_gdtm)
            {
                Match PicAddr = Regex.Match(m_source.Substring(gdtm.Index), @"<a href=.*?>");
                m_PicList.Add( PicAddr.Value.Substring(9).TrimEnd(">\"".ToCharArray()));
            }
        }

        public GalleryPage(string URL,int MPage,int CPage,DownloadContext Context) :base(URL)
        {
            m_PicList=new List<string>();
            FetchPicList();
            DbgFormFactory.GetDbgForm().WriteLine(string.Format("\nPicture pages of gallery {0}:\n",URL));
            foreach (string PicPageURL in m_PicList)
                DbgFormFactory.GetDbgForm().WriteLine(PicPageURL);
            int PicNum = 0;
            foreach (string PicPageURL in m_PicList)
            {
                Context.m_URL = new PicPage(PicPageURL).m_PicURL;
                Context.m_Notify(MPage, CPage, m_PicList.Count, ++PicNum, Context);
                if(Context.m_Downloader!=null)
                    Context.m_Downloader.AddFile(PicPageURL, Context.m_URL);
            }
            
        }
    }

    public class FrontPage : WebPage
    {

        private List<string> m_PageList;

        private string GetGalleryName()
        {
            Match Tag_gn=Regex.Match(m_source,@"<h1 id=""gn"">.*?<");
            return Tag_gn.Value.Substring(12).TrimEnd('<');
        }

        private void FetchPageList()
        {
            MatchCollection Results = Regex.Matches(m_source, @"sp\(([0-9]*)\)");
            int PageNum = 0;
            foreach (Match Raw in Results)
            {
                int tmp = int.Parse(Raw.Value.Trim("sp()".ToCharArray()));
                if (tmp > PageNum)
                    PageNum = tmp;
            }

            m_PageList.Add(m_URL);
            for (int i = 1; i <= PageNum; ++i)
                m_PageList.Add(string.Format("{0}/?p={1}", m_URL, i));
        }

        public FrontPage(string URL,DownloadContext Context) : base(URL)
        {
            m_PageList = new List<string>();
            m_URL = URL.TrimEnd('/');
            string Library;
            MainForm.GetLibraryPath(out Library);

            string GalleryName=GetGalleryName();
            MainForm.Me.SetGalleryTitle(GalleryName);
            
            if(Context.m_Downloader!=null)
                Context.m_Downloader.GalleryName = GalleryName;

            FetchPageList();
            DbgFormFactory.GetDbgForm().WriteLine("Page List:");
            foreach (string GalleryPageURL in m_PageList)
                DbgFormFactory.GetDbgForm().WriteLine(GalleryPageURL);
            int Page=0;
            foreach (string GalleryPageURL in m_PageList)
                new GalleryPage(GalleryPageURL,m_PageList.Count,++Page,Context);

            if (Context.m_Downloader != null)
                Context.m_Downloader.Done = true;
            MainForm.Me.Invoke(new MethodInvoker(delegate { MainForm.Me.Abort(); }));
        }
    }
}
