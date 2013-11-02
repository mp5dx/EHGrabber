using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace EHGrabber
{
    public partial class LoginForm : Form
    {
        private static string LoginURL = "http://e-hentai.org/bounce_login.php?b=d&bt=1-1";
        private static string PostData = "ipb_login_username={0}&ipb_login_password={1}&ipb_login_submit=Login!";
        private static string UserID=null;
        private static string PwHash=null;

        public static bool Logined
        {
            get { return UserID != null && PwHash != null; }
        }

        public static string ID
        {
            get { return UserID; }
        }
        public static string Password
        {
            get { return PwHash; }
        }

        public LoginForm()
        {
            InitializeComponent();
        }

        public static bool GetRegKey(out string Username,out string Password)
        {
            string retID=(string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "ID", null);
            string retPW = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "PW", null);
            if (retID == null || retPW == null)
            {
                Username =Password= null;
                return false;
            }
            Username = retID;
            Password = retPW;
            return true;
        }

        public static void ClearRegKey()
        {
            RegistryKey Key = Registry.CurrentUser;
            RegistryKey MyKey=Key.OpenSubKey(@"SOFTWARE\EHGrabber",true);
            MyKey.DeleteValue("ID");
            MyKey.DeleteValue("PW");
        }

        public static void AddRegKey(string Username, string Password)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "ID", Username);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\EHGrabber", "PW", Password);
        }

        private static bool CheckLogin(string Source,string Cookie)
        {
            if (Source.IndexOf("This page requires you") != -1 || Cookie.Length==0)
                return false;
            return true;
        }

        private static void GetLoginData(string CookieString)
        {
            Match ID = Regex.Match(CookieString, @"ipb_member_id=[0-9]*");
            UserID = ID.Value.Substring(14);
            Match PW = Regex.Match(CookieString, @"ipb_pass_hash=[0-9a-zA-z]*");
            PwHash = PW.Value.Substring(14);
        }

        public static void Logout()
        {
            UserID = PwHash = null;
        }

        public static bool Login(string Username, string Password)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(LoginURL);
            HttpWebResponse Response;
            CookieContainer Cookies = new CookieContainer();

            Request.Method = "POST";
            Request.KeepAlive = true;
            Request.ContentType = "text/html";
            Request.CookieContainer = Cookies;
            Request.Referer = LoginURL;

            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] ByteData = Encoding.GetBytes(string.Format(PostData, Username, Password));
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = ByteData.Length;
            Stream WriteStream = Request.GetRequestStream();
            WriteStream.Write(ByteData, 0, ByteData.Length);

            Response = (HttpWebResponse)Request.GetResponse();
            if (Request.HaveResponse)
            {
                StreamReader Stream = new StreamReader(Response.GetResponseStream());
                string ResponseString = Stream.ReadToEnd();
                Stream.Close();
                Response.Close();
                string CookieString = Response.Headers["Set-cookie"];

                if (CheckLogin(ResponseString, CookieString))
                {
                    //CookieBox = Response.Cookies;
                    GetLoginData(CookieString);
                    AddRegKey(Username, Password);
                    return true;
                }
                else
                {
                    CookieString = null;
                    MessageBox.Show("Login failed.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Server does not respond");
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login(textBox1.Text, textBox2.Text))
                Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 200, NativeMethods.AW_BLEND);
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UserID == null)
                DialogResult = DialogResult.No;
            else
                DialogResult = DialogResult.OK;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 200, NativeMethods.AW_BLEND | NativeMethods.AW_HIDE);
        }
    }
}
