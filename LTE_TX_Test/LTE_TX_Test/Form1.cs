using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace LTE_TX_Test
{
    public partial class Form1 : Form
    {
        AutoTester TestObj;
        Calibrater CalibrateObj;
        SysConfigure ConfigureObj;

        Thread TestThread=null;


        public delegate void RichMessageInvoke(string message);
        public delegate void ModeLabelInvoke(string mode);

        public Form1()
        {
            ConfigureObj = new SysConfigure();
            CalibrateObj = new Calibrater(this, ConfigureObj);
            TestObj = new AutoTester(this, ConfigureObj);


            InitializeComponent();
        }

        private void closeThread()
        {
            if (TestThread != null)
            {
                if (TestThread.IsAlive)
                {
                    TestThread.Abort();
                }
            }
        }  

        public void UpdateRichMultiThread(string info)
        {
            RichMessageInvoke Mi = new RichMessageInvoke(UpdateRichMessage);

            this.BeginInvoke(Mi, new object[] { info });

        }

        public void UpdateRichMessage(string info)
        {
            StringBuilder sb= new StringBuilder();
            sb.Append(info);
            richTextBox1.AppendText(sb.ToString());
            
        }

        public void UpdateModemMultiThread(string info)
        {
            ModeLabelInvoke Mi = new ModeLabelInvoke(UpdateModemMode);

            this.BeginInvoke(Mi, new object[] { info });

        }

        public void UpdateModemMode(string info)
        {
            ModemMode_Label.Text = "Modem Mode:" + info;


        }

        private void regToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            closeThread();

           TestThread= new Thread(new ThreadStart(AutoRun)) ;

           
            TestThread.Start();
           

            
        }


        private void AutoRun()
        {
            TestObj.RunTest();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 校准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm SF1 = new SettingForm(ConfigureObj);
            SF1.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeThread();
        }
    }
}
