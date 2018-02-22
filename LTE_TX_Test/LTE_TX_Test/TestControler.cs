using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace LTE_TX_Test
{
    class TestControler
    {
        Form1 MessageForm;
        DUT DUTObj;
        Instrument InstrumentObj;
        SysConfigure ConfigurePara;

        double Result=23.00;
        string ErrorCode="0";// "0" : No Error

        public TestControler(Form1 f1,SysConfigure sc1)
        {
            MessageForm = f1;
            ConfigurePara = sc1;
            DUTObj = new DUT();
            InstrumentObj = new Instrument();
        }

        public double GetResult()
        {
            
          double Freq=0, Pwr=0;

          

          InstrumentObj.GetAvePwr(ref Freq,ref Pwr);
           
                       
            
            return Pwr;
        }

        
       


        public bool RunTest()
        {
            bool result = true;
            if (DUTObj.OpenDiagPort() > 0)
            {
                //MessageForm.UpdateModemMode(DUTObj.GetModemMode());

                MessageForm.UpdateModemMultiThread(DUTObj.GetModemMode());


                string SN=DUTObj.GetSN();

                //MessageForm.UpdateRichMessage(SN+":\n");

                MessageForm.UpdateRichMultiThread(SN + ":\n");

                DUTObj.TransPwr(18300);

                double Freq = 0, Pwr = 0;

                InstrumentObj.GetAvePwr(ref Freq, ref Pwr);

                Pwr=Pwr+ConfigurePara.InsertLossB1;

                //MessageForm.UpdateRichMessage(Convert.ToString(Freq) +"Hz  "+ Convert.ToString(Pwr) + "dBm  ");
                MessageForm.UpdateRichMultiThread(Convert.ToString(Freq) + "Hz  " + Convert.ToString(Pwr) + "dBm  ");


                if ((Pwr >= ConfigurePara.MinLimit) && (Pwr <= ConfigurePara.MaxLimit))
                    //MessageForm.UpdateRichMessage("Pass\n");
                    MessageForm.UpdateRichMultiThread("Pass\n");
                else
                {
                    //MessageForm.UpdateRichMessage("Fail\n");
                    MessageForm.UpdateRichMultiThread("Fail\n");
                    result = false;
                }

                
                DUTObj.PowOff();

                DUTObj.TransPwr(21625);

               
                InstrumentObj.GetAvePwr(ref Freq, ref Pwr);

                Pwr = Pwr + ConfigurePara.InsertLossB8;

                //MessageForm.UpdateRichMessage(" "+Convert.ToString(Freq) + "Hz  " + Convert.ToString(Pwr) + "dBm  ");


                MessageForm.UpdateRichMultiThread(" " + Convert.ToString(Freq) + "Hz  " + Convert.ToString(Pwr) + "dBm  ");

                DUTObj.PowOff();

                if ((Pwr >= ConfigurePara.MinLimit) && (Pwr <= ConfigurePara.MaxLimit))
                    //MessageForm.UpdateRichMessage("Pass\n");
                    MessageForm.UpdateRichMultiThread("Pass\n");
                else
                {
                    //MessageForm.UpdateRichMessage("Fail\n");

                    MessageForm.UpdateRichMultiThread("Fail\n");
                    result = false;
                }





            }

            else
            {
                //MessageForm.UpdateRichMessage("DUT Open Error!\n");

                MessageForm.UpdateRichMultiThread("DUT Open Error!\n");
                return false;
            }


            
             DUTObj.SetModemMode(DUTObj.OriginalMode);

             
             DUTObj.Disconnect();

             return result;
            

          
        }

        


    }
}
