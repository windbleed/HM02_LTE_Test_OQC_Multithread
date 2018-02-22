using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTE_TX_Test
{
    class Calibrater
    {
        Form1 MessageForm;
        SysConfigure ConfigurePara;
        TestControler TCObj;

        public Calibrater(Form1 F1,SysConfigure CP1)
        {
            MessageForm = F1;
            ConfigurePara = CP1;

            TCObj = new TestControler(F1,CP1);


        }

        public void DoCalibrate()
        {

        }
    }
}
