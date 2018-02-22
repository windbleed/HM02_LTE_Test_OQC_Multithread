using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTE_TX_Test
{
    class AutoTester
    {
        TestControler TCObj;
        Form1 MessageForm;
        SysConfigure ConfigurePara;

        public AutoTester(Form1 F1, SysConfigure SC1)
        {
            MessageForm = F1;
            ConfigurePara = SC1;

            TCObj = new TestControler(F1,SC1);
        }


        public void RunTest()
        {
            
            
            ConfigurePara.GetConfigure();


            if (TCObj.RunTest())
                MessageBox.Show("测试结果PASS！");
            else MessageBox.Show("测试结果Fail!");
                    



                
           

           

        }


    }
}
