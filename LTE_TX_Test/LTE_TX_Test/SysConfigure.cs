using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;


namespace LTE_TX_Test
{
   public class SysConfigure
    {
       public double InsertLossB1 =   0.00;
       public double InsertLossB8 =   0.00;

       public double MaxLimit =   25.00;
       public double MinLimit =   18.00;
       
        static string ConfigureFile="config.xml";

        public void SetConfigure(float IS,float Max,float Min)
        {
            //save  para to file

        }

        public void GetConfigure()
        {

          InsertLossB1= Convert.ToDouble(GetNodeValue("InsertLossB1"));
          InsertLossB8 = Convert.ToDouble(GetNodeValue("InsertLossB8"));
          MaxLimit = Convert.ToDouble(GetNodeValue("MaxLimit"));
          MinLimit = Convert.ToDouble(GetNodeValue("MinLimit"));


        }

       public void SetConfigure()
        {

            UpdateSetValue("InsertLossB1",Convert.ToString(InsertLossB1));
            UpdateSetValue("InsertLossB8", Convert.ToString(InsertLossB8));
            UpdateSetValue("MaxLimit", Convert.ToString(MaxLimit));
            UpdateSetValue("MinLimit", Convert.ToString(MinLimit));

        }




        public static string GetNodeValue(string tagName)
        {
            string result = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigureFile); 

             
            XmlNodeList nodes = xmlDoc.SelectSingleNode("Config").ChildNodes;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name.Equals(tagName))
                {
                    
                    result = nodes[i].InnerText;
                    return result;
                }
            }
            return result;
        }

        public static void UpdateSetValue(string tagName, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigureFile); 
            // XmlNode node = xmlDoc.GetElementById("Config");  
            XmlNodeList nodes = xmlDoc.SelectSingleNode("Config").ChildNodes;

            for (int i = 0; i < nodes.Count; i++)
            {
                //MessageBox.Show(nodes[i].Name);
                if (nodes[i].Name.Equals(tagName))
                {
                    nodes[i].InnerText = value;
                }
            }
            try
            {
                
                xmlDoc.Save("config.xml");

            }
            catch (Exception e)
            {
                throw e;
            }
        }  



    }
}
