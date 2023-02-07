using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MqttServer
{
    public partial class Form1 : Form
    {
        public static UInt16 u16RcvedMqttServerStatus;

        string btn1_resourceKey = "OFF";
        string btn2_resourceKey = "OFF";
        string btn3_resourceKey = "OFF";
        string btn4_resourceKey = "OFF";
        string btn5_resourceKey = "OFF";
        string btn6_resourceKey = "OFF";
        string btn7_resourceKey = "OFF";
        string btn8_resourceKey = "OFF";
        string btn9_resourceKey = "OFF";
        string btn10_resourceKey = "OFF";

        Timer CheckRadioBtnTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            MqttServer.bMqttServerWored = false;
            u16RcvedMqttServerStatus = 0;

            CheckRadioBtnTimer.Interval = 1000;
            CheckRadioBtnTimer.Tick += new EventHandler(CheckRadioBtnTimer_Tick);

            CheckRadioBtnTimer.Start();
        }

        private void CheckRadioBtnTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("CheckRadioBtnTimer_Tick Function bMqttServerWored : {0} u16RcvedMqttServerStatus : {1}", MqttServer.bMqttServerWored , u16RcvedMqttServerStatus);

            if ((MqttServer.bMqttServerWored == false) && (u16RcvedMqttServerStatus != 0))
            {

            }
            else if ((MqttServer.bMqttServerWored == true) && (u16RcvedMqttServerStatus == 0))
            {
                if (MqttServer.bMqttServerReceived == true)
                {
                    label10.Text = MqttServer.Location_list.Count.ToString();
                }
            }
            else if ((MqttServer.bMqttServerWored == true) && (u16RcvedMqttServerStatus != 0))
            {
                if (MqttServer.bMqttServerReceived == true)
                {
                    label10.Text = MqttServer.Location_list.Count.ToString();

                    if (Check_Bit(u16RcvedMqttServerStatus , 0))
                    {
                        string IPList = MqttServer.Location_list[0];
                        textBox1.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox11.Text = Value;

                        progressBar1.Value = Convert.ToUInt16(Value);
                        pictureBox11.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 1))
                    {
                        string IPList = MqttServer.Location_list[1];
                        textBox2.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox12.Text = Value;

                        progressBar2.Value = Convert.ToUInt16(Value);
                        pictureBox12.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 2))
                    {
                        string IPList = MqttServer.Location_list[2];
                        textBox3.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox13.Text = Value;

                        progressBar3.Value = Convert.ToUInt16(Value);
                        pictureBox13.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 3))
                    {
                        string IPList = MqttServer.Location_list[3];
                        textBox4.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox14.Text = Value;

                        progressBar4.Value = Convert.ToUInt16(Value);
                        pictureBox14.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 4))
                    {
                        string IPList = MqttServer.Location_list[4];
                        textBox5.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox15.Text = Value;

                        progressBar5.Value = Convert.ToUInt16(Value);
                        pictureBox15.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 5))
                    {
                        string IPList = MqttServer.Location_list[5];
                        textBox6.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox16.Text = Value;

                        progressBar6.Value = Convert.ToUInt16(Value);
                        pictureBox16.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 6))
                    {
                        string IPList = MqttServer.Location_list[7];
                        textBox7.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox17.Text = Value;

                        progressBar7.Value = Convert.ToUInt16(Value);
                        pictureBox17.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 7))
                    {
                        string IPList = MqttServer.Location_list[8];
                        textBox8.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox18.Text = Value;

                        progressBar8.Value = Convert.ToUInt16(Value);
                        pictureBox18.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 8))
                    {
                        string IPList = MqttServer.Location_list[9];
                        textBox9.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox19.Text = Value;

                        progressBar9.Value = Convert.ToUInt16(Value);
                        pictureBox19.BackgroundImage = Properties.Resources.ONLINE;
                    }

                    if (Check_Bit(u16RcvedMqttServerStatus, 9))
                    {
                        string IPList = MqttServer.Location_list[10];
                        textBox10.Text = IPList;

                        string Value = MqttServer.dictLocation[IPList];
                        textBox20.Text = Value;

                        progressBar10.Value = Convert.ToUInt16(Value);
                        pictureBox20.BackgroundImage = Properties.Resources.ONLINE;
                    }
                }
            }
        }

        public static bool Check_Bit(UInt16 _data, int loc)
        {
            int val = (0x1 << loc);
            return ((int)_data & val) == val;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((btn1_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count != 0))
            {
                u16RcvedMqttServerStatus |= 0x0001;
                btn1_resourceKey = "ON";
                button1.BackgroundImage = Properties.Resources.ON_1;
                pictureBox21.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfffe;
                btn1_resourceKey = "OFF";
                button1.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox21.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((btn2_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 2))
            {
                u16RcvedMqttServerStatus |= 0x0002;
                btn2_resourceKey = "ON";
                button2.BackgroundImage = Properties.Resources.ON_1;
                pictureBox22.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfffd;
                btn2_resourceKey = "OFF";
                button2.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox22.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((btn3_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 3))
            {
                u16RcvedMqttServerStatus |= 0x0004;
                btn3_resourceKey = "ON";
                button3.BackgroundImage = Properties.Resources.ON_1;
                pictureBox23.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfffb;
                btn3_resourceKey = "OFF";
                button3.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox23.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((btn4_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 4))
            {
                u16RcvedMqttServerStatus |= 0x0008;
                btn4_resourceKey = "ON";
                button4.BackgroundImage = Properties.Resources.ON_1;
                pictureBox24.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfff7;
                btn4_resourceKey = "OFF";
                button4.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox24.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if ((btn5_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 5))
            {
                u16RcvedMqttServerStatus |= 0x0010;
                btn5_resourceKey = "ON";
                button5.BackgroundImage = Properties.Resources.ON_1;
                pictureBox25.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xffef;
                btn5_resourceKey = "OFF";
                button5.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox25.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if ((btn6_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 6))
            {
                u16RcvedMqttServerStatus |= 0x0020;
                btn6_resourceKey = "ON";
                button6.BackgroundImage = Properties.Resources.ON_1;
                pictureBox26.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xffdf;
                btn6_resourceKey = "OFF";
                button6.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox26.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        { 
            if ((btn7_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 7))
            {
                u16RcvedMqttServerStatus |= 0x0040;
                btn7_resourceKey = "ON";
                button7.BackgroundImage = Properties.Resources.ON_1;
                pictureBox27.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xffbf;
                btn7_resourceKey = "OFF";
                button7.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox27.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if ((btn8_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 8))
            {
                u16RcvedMqttServerStatus |= 0x0080;
                btn8_resourceKey = "ON";
                button8.BackgroundImage = Properties.Resources.ON_1;
                pictureBox28.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xff7f;
                btn8_resourceKey = "OFF";
                button8.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox28.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if ((btn9_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 9))
            {
                u16RcvedMqttServerStatus |= 0x0100;
                btn9_resourceKey = "ON";
                button9.BackgroundImage = Properties.Resources.ON_1;
                pictureBox29.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfeff;
                btn9_resourceKey = "OFF";
                button9.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox29.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if ((btn10_resourceKey == "OFF") && (MqttServer.bMqttServerReceived == true) && (MqttServer.Location_list.Count >= 10))
            {
                u16RcvedMqttServerStatus |= 0x0200;
                btn10_resourceKey = "ON";
                button10.BackgroundImage = Properties.Resources.ON_1;
                pictureBox30.BackgroundImage = Properties.Resources.Button_Blank_Green_Icon_72;
            }
            else
            {
                u16RcvedMqttServerStatus &= 0xfdff;
                btn10_resourceKey = "OFF";
                button10.BackgroundImage = Properties.Resources.OFF_1;
                pictureBox30.BackgroundImage = Properties.Resources.Button_Blank_Gray_Icon_72;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // Default Port = 1883
            MqttServer.strMQTTServerPort = tbPort.Text;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Rectangle r = e.CellBounds;
            using (Pen pen = new Pen(Color.Black))
            {
                // top and left lines
                e.Graphics.DrawLine(pen, r.X, r.Y, r.X + r.Width, r.Y);
                e.Graphics.DrawLine(pen, r.X, r.Y, r.X, r.Y + r.Height);
                // last row? move hor.lines 1 up!
                int cy = e.Row == tableLayoutPanel1.RowCount - 1 ? -1 : 0;
                if (cy != 0) e.Graphics.DrawLine(pen, r.X, r.Y + r.Height + cy,
                                        r.X + r.Width, r.Y + r.Height + cy);
                // last column ? move vert. lines 1 left!
                int cx = e.Column == tableLayoutPanel1.ColumnCount - 1 ? -1 : 0;
                if (cx != 0) e.Graphics.DrawLine(pen, r.X + r.Width + cx, r.Y,
                                        r.X + r.Width + cx, r.Y + r.Height);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine("CheckRadioBtnTimer_Tick MqttSever Work Start !!!");
            btnStart.BackColor = Color.FromArgb(192, 255, 192);
            btnStop.BackColor = Color.White;
            MqttServer.MqttStart();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("CheckRadioBtnTimer_Tick MqttSever Work Stop !!!");
            btnStop.BackColor = Color.FromArgb(255, 192, 192);
            btnStart.BackColor = Color.White;

            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox8.Text = textBox9.Text = textBox10.Text = "";
            textBox11.Text = textBox12.Text = textBox13.Text = textBox14.Text = textBox15.Text = textBox16.Text = textBox17.Text = textBox18.Text = textBox19.Text = textBox20.Text = "";

            progressBar1.Value = progressBar2.Value = progressBar3.Value = progressBar4.Value = progressBar5.Value = progressBar6.Value = progressBar7.Value = progressBar8.Value = progressBar9.Value = 0;
            pictureBox11.BackgroundImage = pictureBox12.BackgroundImage = pictureBox13.BackgroundImage = pictureBox14.BackgroundImage = pictureBox15.BackgroundImage = pictureBox16.BackgroundImage = pictureBox17.BackgroundImage = Properties.Resources.OFFLINE;
            pictureBox18.BackgroundImage = pictureBox19.BackgroundImage = pictureBox20.BackgroundImage = Properties.Resources.OFFLINE;

            label10.Text = "0";

            MqttServer.MqttStop();
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            Message watch = Message.GetInstance;
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(Message))
                {
                    watch.Activate();
                    return;
                }
            }
            watch.Show();
        }
    }
}
