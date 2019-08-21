using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetClassicTheme
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public Form1()
        {
            InitializeComponent();
        }

        List<(string, IntPtr)> windowss;
        private void Form1_Load(object sender, EventArgs e)
        {
            windowss = GetWindows();
            foreach((string, IntPtr) p in windowss)
            {
                listBox1.Items.Add(p.Item1);
            }
        }

        public string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }
            return String.Empty;
        }
        public List<(string, IntPtr)> GetWindows()
        {
            IntPtr found = IntPtr.Zero;
            List<(string, IntPtr)> windows = new List<(string, IntPtr)>();
            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                string title = GetWindowText(wnd);
                if (title == null) title = "";
                windows.Add((title, wnd));
                return true;
            }, IntPtr.Zero);
            return windows;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SetWindowTheme(windowss[listBox1.SelectedIndex].Item2, " ", " ");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SelectedIndex = i;
                Button1_Click(sender, e);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Form1_Load(sender, e);
        }
    }
}
