using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTE_TX_Test
{
    public partial class SettingForm : Form
    {
         SysConfigure ConfigData;

        public SettingForm(SysConfigure SC1)
        {
            InitializeComponent();
            ConfigData = SC1;
            ConfigData.GetConfigure();
            ILB1_Text.Text = Convert.ToString(ConfigData.InsertLossB1);
            ILB8_Text.Text = Convert.ToString(ConfigData.InsertLossB8);
            Max_Text.Text = Convert.ToString(ConfigData.MaxLimit);
            Min_Text.Text= Convert.ToString(ConfigData.MinLimit);

        }

        


        private void button1_Click(object sender, EventArgs e)
        {
           // ConfigData.GetConfigure();
           // ILB1_Text = Convert.ToString(ConfigData.InsertLossB1);
            ConfigData.InsertLossB1 = Convert.ToDouble(ILB1_Text.Text);
            ConfigData.InsertLossB8 = Convert.ToDouble(ILB8_Text.Text);
            ConfigData.MaxLimit = Convert.ToDouble(Max_Text.Text);
            ConfigData.MinLimit = Convert.ToDouble(Min_Text.Text);

            ConfigData.SetConfigure();

            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
