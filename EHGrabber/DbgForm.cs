using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EHGrabber
{


    public partial class DbgForm : Form
    {
        public DbgForm()
        {
            InitializeComponent();
        }
        public DbgForm(string HtmlSrc)
        {
            InitializeComponent();
            richTextBox1.Text = HtmlSrc;
        }

        public void WriteLine(string Line)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new MethodInvoker(delegate() { WriteLine(Line); }));
            }
            else
            richTextBox1.Text += Line + "\n";
        }

        public void Clear()
        {
            richTextBox1.Text = "";
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void DbgForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            NativeMethods.AnimateWindow(this.Handle, 200, NativeMethods.AW_BLEND | NativeMethods.AW_HIDE);
            this.Hide();
        }

        private void DbgForm_Load(object sender, EventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 200, NativeMethods.AW_BLEND);
        }

    }
    public static class DbgFormFactory
    {
        private static DbgForm m_DbgForm = null;

        public static DbgForm GetDbgForm()
        {
            if (m_DbgForm == null)
                m_DbgForm = new DbgForm();
            return m_DbgForm;
        }
    }
}
