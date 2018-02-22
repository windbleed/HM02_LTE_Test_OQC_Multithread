using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace LTE_TX_Test
{
    class DUT
    {

        IntPtr QLIB_COM;
        string SN="88888888888888";
        int DiagPort=0;

        public  string OriginalMode = "null";

        const int MODE_FTM_F = 3;
        const short PHONE_MODE_LTE_B1 = 34;
        const short PHONE_MODE_LTE_B8 = 47;
        const ushort FTM_ON = 1;

        const ushort Pdm = 65;

        

        
        public bool GetReady()
        {
            return true;
        }

        
        public void TransPwr(ushort channel)
        {

            ushort result_pa_status=0;

            QLIB_DIAG_CONTROL_F(QLIB_COM, MODE_FTM_F);

            if ((channel >= 18050) && (channel <= 18500))

                QLIB_FTM_SET_MODE(QLIB_COM, PHONE_MODE_LTE_B1);

            else

                QLIB_FTM_SET_MODE(QLIB_COM, PHONE_MODE_LTE_B8);

            QLIB_FTM_LTE_SET_TX_BANDWIDTH(QLIB_COM, 3);

            QLIB_FTM_LTE_SET_RX_BANDWIDTH(QLIB_COM, 3);

            QLIB_FTM_SET_CHAN(QLIB_COM, channel);
            
            QLIB_FTM_SET_SECONDARY_CHAIN(QLIB_COM, FTM_ON);

            QLIB_FTM_LTE_SET_TX_MODULATION_TYPE(QLIB_COM, 0);

            QLIB_FTM_SET_TX_ON(QLIB_COM);

            QLIB_FTM_LTE_SET_TX_WAVEFORM(QLIB_COM, 1, 50, 0, 0);

            QLIB_FTM_SET_PA_RANGE(QLIB_COM, 0);

            QLIB_FTM_SET_PA_STATE(QLIB_COM, 1,  ref result_pa_status);


            QLIB_FTM_SET_TX_GAIN_INDEX(QLIB_COM, Pdm);







        }

        public void PowOff()
        {
            QLIB_FTM_SET_TX_OFF(QLIB_COM);
        }


        public int GetDiagPort()
        {
          

            ushort NumPorts, NumIgnorePorts;
            ushort[] PortList = {0,0,0,0,0};
            ushort[] IgnorePortList = {1,2};

           
            NumIgnorePorts = 2;
            NumPorts = 5;

        
            if (QLIB_GetAvailablePhonesPortList(ref NumPorts, ref PortList[0], NumIgnorePorts, ref IgnorePortList[0]) != 0)
            {
                if (NumPorts > 0)
                {
                    return Convert.ToInt16(PortList[0]);


                }



            }
           
            return (-1);

        }


        public int  OpenDiagPort()
        {
           
            int PortNum=GetDiagPort();


            if(PortNum>0)
            {
                 QLIB_COM = QLIB_ConnectServerWithWait((uint)PortNum, 2000);

                 //OriginalMode = GetModemMode();
                
                return 1;
            }

            return -1;
        }

        public void Disconnect()
        {
            if(QLIB_COM!=null)

                QLIB_DisconnectServer(QLIB_COM);
        }

        public string GetSN()
        {
            
            byte[] SN = new byte[12];
            ushort status = 0;
            ushort result = 0;
            result = QLIB_DIAG_NV_READ_F(QLIB_COM, 2824, SN, 12, ref status);
            string s2 = System.Text.Encoding.Default.GetString(SN);

          
            byte ConnectStatus;
            ConnectStatus = QLIB_IsPhoneConnected(QLIB_COM);

            if (ConnectStatus == 0)
                return ("Fail");
           
             return (s2);
        }

   public string GetModemMode()
   {

       string ModemMode="NULL";
       
        byte result;
        ulong modemMode=0;

//acquire current operation mode

        result=QLIB_GetPhoneOperatingMode(QLIB_COM, ref modemMode);
/*


        SYS_OPRT_MODE_PWROFF        = 0,   //!< ' Phone is powering off
        SYS_OPRT_MODE_FTM           = 1,   //!< ' Phone is in factory test mode
        SYS_OPRT_MODE_OFFLINE       = 2,   //!< ' Phone is offline
        SYS_OPRT_MODE_OFFLINE_AMPS  = 3,   //!< ' Phone is offline analog
        SYS_OPRT_MODE_OFFLINE_CDMA  = 4,   //!< ' Phone is offline cdma
        SYS_OPRT_MODE_ONLINE        = 5,   //!< ' Phone is online
        SYS_OPRT_MODE_LPM           = 6,   //!< ' Phone is in LPM - Low Power Mode
        SYS_OPRT_MODE_RESET         = 7,   //!< ' Phone is resetting - i.e. power-cycling
        SYS_OPRT_MODE_NET_TEST_GW   = 8,   //!< ' Phone is conducting network test for GSM/WCDMA.
        SYS_OPRT_MODE_OFFLINE_IF_NOT_FTM = 9, //!< ' offline request during powerup.
        SYS_OPRT_MODE_PSEUDO_ONLINE = 10, //!< ' Phone is pseudo online, tx disabled

        modemMode=0 Powering off
        modemMode=1 FTM mode
        modemMode=5 online mode
   */
        if (result!=0)
        {

                switch(modemMode)
                {
                        case 0:  ModemMode="Powering off";      break;
                        case 1:  ModemMode="FTM mode";          break;
                        case 2:  ModemMode="Offline mode";      break;
                        case 3:  ModemMode="Offline analog";    break;
                        case 4:  ModemMode="offline cdma";      break;
                        case 5:  ModemMode="Online mode";       break;
                        case 6:  ModemMode="Low Power Mode";    break;
                        case 7:  ModemMode="Reset...";          break;
                        case 8:  ModemMode="network test...";   break;
                        case 9:  ModemMode="offline request...";break;
                        case 10: ModemMode="Pseudo online";     break;
                        default: ModemMode=" ";                 break;
                }
        }
        OriginalMode = ModemMode;
         return ModemMode;
   }


   public bool SetModemMode(string mode)
   {
        byte result;

        int setmode=0;

        switch (mode)
        {
            case "Powering off":    setmode = 6; break;
            case "FTM mode":        setmode = 3; break;
            case "Offline mode":    setmode = 1; break;
            case "Offline analog":  setmode = 0; break;
            case "Online mode":     setmode = 4; break;
            case "Low Power Mode":  setmode = 5; break;
            case "offline cdma":    setmode = 1; break;

            default:                setmode = 5; break;
                              

        }


      /*

        MODE_OFFLINE_A_F = 0,    //!<' Go to offline analog
        MODE_OFFLINE_D_F = 1,    //!<' Go to offline digital
        MODE_RESET_F = 2,        //!<' Reset. Only exit from offline
        MODE_FTM_F = 3,          //!<' FTM mode
        MODE_ONLINE_F = 4,       //!<' Go to Online
        MODE_LPM_F = 5,          //!<' Low Power Mode (if supported)
        MODE_POWER_OFF_F = 6,    //!<' Power off (if supported)
        MODE_MAX_F = 7           //!<' Last (and invalid) mode enum value

        */

        result=QLIB_DIAG_CONTROL_F(QLIB_COM,setmode);

        if (result==0)  return false;
         
        return true;

   
   }







        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_GetAvailablePhonesPortList", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_GetAvailablePhonesPortList
            (ref ushort iNumPorts, ref ushort pPortList, ushort iNumIgnorePorts, ref ushort pIgnorePortList);

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_DIAG_NV_READ_F", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_DIAG_NV_READ_F
            (System.IntPtr hResourceContext, ushort iItemID, byte[] pItemData, int iLength, ref ushort iStatus);

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_ConnectServerWithWait", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr QLIB_ConnectServerWithWait
            (uint iComPort, ulong iWait_msorePortList);


        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_IsPhoneConnected", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_IsPhoneConnected
            (System.IntPtr hResourceContext);


        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_DIAG_CONTROL_F", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_DIAG_CONTROL_F
            (System.IntPtr hResourceContext, int eMode);

        //QLIB_API unsigned char QLIB_DIAG_CONTROL_F(HANDLE hResourceContext, int eMode );

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_MODE", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_MODE
            (System.IntPtr hResourceContext, short eNewMode);

        //QLIB_API unsigned char QLIB_FTM_SET_MODE( HANDLE hResourceContext, short eNewMode );  

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_LTE_SET_TX_BANDWIDTH", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_LTE_SET_TX_BANDWIDTH
            (System.IntPtr hResourceContext, byte itxChanBW);

        //QLIB_API unsigned char QLIB_FTM_LTE_SET_TX_BANDWIDTH( HANDLE hResourceContext, unsigned char itxChanBW );
        
        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_LTE_SET_RX_BANDWIDTH", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_LTE_SET_RX_BANDWIDTH
            (System.IntPtr hResourceContext, byte irxChanBW);


       // QLIB_API unsigned char QLIB_FTM_LTE_SET_RX_BANDWIDTH( HANDLE hResourceContext, unsigned char irxChanBW );
    
        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_CHAN", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_CHAN
            (System.IntPtr hResourceContext, ushort iChannel);

        
        //QLIB_API unsigned char QLIB_FTM_SET_CHAN( HANDLE hResourceContext, unsigned short iChannel); 
    
        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_SECONDARY_CHAIN", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_SECONDARY_CHAIN
            (System.IntPtr hResourceContext, ushort iMode);

        //QLIB_API unsigned char QLIB_FTM_SET_SECONDARY_CHAIN( HANDLE hResourceContext, unsigned short iMode ); 
    
        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_LTE_SET_TX_MODULATION_TYPE", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_LTE_SET_TX_MODULATION_TYPE
            (System.IntPtr hResourceContext, ulong iModulationType);

        //QLIB_API unsigned char QLIB_FTM_LTE_SET_TX_MODULATION_TYPE( HANDLE hResourceContext, unsigned long iModulationType);

        
        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_TX_ON", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_TX_ON
            (System.IntPtr hResourceContext);

               
        //QLIB_API unsigned char QLIB_FTM_SET_TX_ON( HANDLE hResourceContext );


        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_LTE_SET_TX_WAVEFORM", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_LTE_SET_TX_WAVEFORM
            (System.IntPtr hResourceContext, byte iTxWaveform,byte inumRBsPUSCH,byte inumRBsPUCCH,byte iPUSCHStartRBIndex);
        
       // QLIB_API unsigned char QLIB_FTM_LTE_SET_TX_WAVEFORM( HANDLE hResourceContext, unsigned char iTxWaveform, unsigned char inumRBsPUSCH, unsigned char inumRBsPUCCH,unsigned char iPUSCHStartRBIndex );

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_PA_RANGE", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_PA_RANGE
            (System.IntPtr hResourceContext,ushort iPArange);


        //QLIB_API unsigned char QLIB_FTM_SET_PA_RANGE( HANDLE hResourceContext, unsigned short iPArange );

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_PA_STATE", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_PA_STATE
            (System.IntPtr hResourceContext,byte iPA_State,ref ushort iFTM_Error_Code);

       // QLIB_API unsigned char QLIB_FTM_SET_PA_STATE( HANDLE hResourceContext, unsigned char iPA_State, unsigned short* iFTM_Error_Code);

        [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_TX_GAIN_INDEX", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_TX_GAIN_INDEX
            (System.IntPtr hResourceContext,ushort iTxGainIndex);


         //QLIB_API unsigned char QLIB_FTM_SET_TX_GAIN_INDEX( HANDLE hResourceContext, unsigned short iTxGainIndex);

         [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_FTM_SET_TX_OFF", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_FTM_SET_TX_OFF
            (System.IntPtr hResourceContext);
        
       // QLIB_API unsigned char QLIB_FTM_SET_TX_OFF( HANDLE hResourceContext );

         [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_DisconnectServer", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_DisconnectServer
            (System.IntPtr hResourceContext);

         //QLIB_API void QLIB_DisconnectServer( HANDLE hResourceContext );

         [DllImport("QMSL_MSVC6R.dll", EntryPoint = "QLIB_GetPhoneOperatingMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte QLIB_GetPhoneOperatingMode
            (System.IntPtr hResourceContext, ref ulong piPhoneMode);

       // QLIB_API unsigned char QLIB_GetPhoneOperatingMode(HANDLE hResourceContext, unsigned long *piPhoneMode);

    }
}
