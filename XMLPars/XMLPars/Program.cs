using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace XMLPars
{

    class Program
    {
        public static int intCount = 0;
        public static string strParentName = "";
        public static string strDelimeter = ";";
        public static List<String> lstWriteToFile = new List<String>();
        static void Main(string[] args)
        {


            XmlDocument doc = new XmlDocument();
            doc.Load("xmlharjoitusHiQ.xml");
            

            TraverseNodes(doc.ChildNodes,"");

            while (doc.NextSibling!=null)
            {
                TraverseNodes(doc.NextSibling.ChildNodes,"");
            }

            WriteRows(lstWriteToFile, "testi.txt");
            
        }

        private static void TraverseNodes(XmlNodeList nodes, String strOldestParentName)
        {
            foreach (XmlNode node in nodes)
            {
                
                if (node.Name=="name")
                {                    
                    lstWriteToFile.Add(intCount.ToString() + strDelimeter +
                    node.InnerText + strDelimeter +
                    strOldestParentName + strDelimeter +
                    strParentName);

                    if (strOldestParentName == "")
                    {
                        strOldestParentName = node.InnerText;
                    }

                    if (node.HasChildNodes && node.FirstChild.Name!="#text")
                    {
                        strParentName= node.InnerText;
                    }
                    intCount++;

                }
                TraverseNodes(node.ChildNodes,strOldestParentName);
            }
        }

        public static void WriteRows(List<string> result, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (string element in result)
                {
                    writer.WriteLine(element);
                }
            }
        }

    }
}
