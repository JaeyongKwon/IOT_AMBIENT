using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MqttServer
{
    public partial class Message : Form
    {
        private static Message instance;
        private static object isLock = new object();

        public Message()
        {
            InitializeComponent();
        }

        public static Message GetInstance
        {
            get
            {
                lock (isLock)
                {
                    if (instance == null || instance.IsDisposed)
                    {
                        instance = new Message();
                    }
                    return instance;
                }
            }
        }

        public void ControlChange(string _recvStr)
        {
            if (txtWatch.InvokeRequired)
            {
                txtWatch.BeginInvoke(new MethodInvoker(delegate ()
                {
                    Console.WriteLine("ControlChange txtWatch BeginInvoke");
                    SetWatchText(_recvStr);
                }));
            }
            else
            {
                Console.WriteLine("ControlChange txtWatch not BeginInvoke");
                txtWatch.Text += _recvStr;
            }
        }

        public void SetWatchText(string _str)
        {
            string _txtProgress = (txtWatch.Text.Length == 0) ? _str : "\r\n" + _str;
            txtWatch.AppendText(_txtProgress);
            txtWatch.ScrollToCaret();
        }

        private void Message_Load(object sender, EventArgs e)
        {

        }
    }
}
