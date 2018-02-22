using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Forms;
using Ivi.Visa.Interop;
using System.Collections;




using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LTE_TX_Test
{
    public class Instrument
    {

        private Ivi.Visa.Interop.FormattedIO488 ioDmm;
        private string AddressGPIB="GPIB::20::1::INSTR";

        private int ErrorCode = 0;// 0 No error 1: instrument open fail

        const string Reset_Cmd              = "SYST:RES:CURR";
        const string SetInputRF1_Cmd        = "INP:STAT RF1";
        const string StartSpecMeasure_Cmd   = "INIT:SPEC";
        const string StopSpecMeasure_Cmd    = "STOP:SPEC";

        const string ReadPwr_Cmd            = "FETC:SPEC:MARK:PEAK?";

        const int N = 5;

        double[] Array_Pwr_Result={0,0,0,0,0};
        double[] Array_Freq_Result={0,0,0,0,0};

        
       

        
     


        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();

        public Instrument()
        {
            ioDmm = new FormattedIO488Class();
            InitInstrument();

        }


        public void SetAddressGPIB(string ADDR)
        {
           
            AddressGPIB = ADDR;
        }



        public void OpenInstrument()
        {
           
            try
            {
                ResourceManager grm = new ResourceManager();
                ioDmm.IO = (IMessage)grm.Open(AddressGPIB, AccessMode.NO_LOCK, 1000, "");
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Open failed on " + AddressGPIB + " " + ex.Source + "  " + ex.Message, "EZSample", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ErrorCode = 1;
                ioDmm.IO = null;
            }

        }

        public bool InitInstrument()
        {
            OpenInstrument();

            if (ErrorCode == 0)
            {
                ioDmm.WriteString(Reset_Cmd, true);
               
                ioDmm.WriteString(SetInputRF1_Cmd, true);
              


                return true;
            }

              

            return false;
        }

      

        public void GetAvePwr(ref double Freq_Ext, ref double Pwr_Ext)
        {
            //Read data from CMU200

            string RespondStr;
            string[] Split_str;
            double Freq;
            double Pwr;

            for (int i = 0; i < N; i++)
            {

                do
                {
                    ioDmm.WriteString(StartSpecMeasure_Cmd, true);
                    Delay(100);
                    ioDmm.WriteString(StopSpecMeasure_Cmd, true);
                    Delay(100);
                    ioDmm.WriteString(ReadPwr_Cmd, true);
                    Delay(100);

                    RespondStr = ioDmm.ReadString();
                } while (RespondStr == "NAN\n");

                
                

                //分离字符串并解析

                Split_str = RespondStr.Split(',');

                Freq    = Convert.ToDouble(Split_str[0]);
                Pwr     = Convert.ToDouble(Split_str[1]);

                Array_Pwr_Result[i] = Pwr;
                Array_Freq_Result[i] = Freq;
                             

            }


            //Array_PWr_Result中取最大值,并几率位置，从该位置Array_Freq_Result读出频率值；

            double temp=Array_Pwr_Result[0];
            int Loc=0;
            for (int i = 1; i < N; i++)
            {
                if (temp < Array_Pwr_Result[i])
                {
                    temp = Array_Pwr_Result[i];
                    Loc = i;
                }

            }

            Pwr_Ext = temp;
            Freq_Ext = Array_Freq_Result[Loc];


         

           // return RespondStr;
        }





        public void SetFreq(int Freq)
        {
            return;
        }
        public void SetIL(int IL)
        {
            return;
        }

        public void SetRBW(int RBW)
        {
            return;
        }

        public void SetVBW(int VBW)
        {
            return;

        }

        public static void Delay(uint ms)
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms)
            {
                Application.DoEvents();
            }
        }



    }
}
