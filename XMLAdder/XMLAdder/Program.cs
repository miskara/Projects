using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace XMLAdder
{
    class Program
    {
        public static XElement root = null;
        public static XElement str = null;
        public static string name = "";
        static void Main()
        {
            string spath = "xmlharjoitusHiQ.xml";
            name = "neljästesti";
            XDocument doc = XDocument.Load(spath);
            str = XElement.Parse(doc.ToString());
            List<XElement> result = new List<XElement>();

            result = str.Elements("item").
            Where(x => x.Element("name").Value.Equals("foo46")).ToList();

            if (result.Count>0)
            {
                root = result[0];
                AddElement(str);
            }

            FindElement(result, str.Elements());

            str.Save(spath);
        }

        private static void FindElement(List<XElement> result, IEnumerable<XElement> elements)
        {
            foreach (XElement el in elements)
            {
                result = el.Elements("item").
                Where(x => x.Element("name").Value.Equals("foo46")).ToList();

                if (result.Count > 0)
                {
                    root = result[0];
                    AddElement(root.Parent);
                    break;
                }
                else
                {
                    FindElement(result, el.Elements());
                }
            }
        }

        public static void AddElement(XElement el) 
        {
            if (root.HasElements && root.Descendants("children").Any())
            {
                XElement newEle = new XElement("item");
                newEle.Add(new XElement("name", name));
                el.Element(root.Name).Element("children").Add(newEle);
            }
            else
            {
                XElement newEle = new XElement("children");
                newEle.Add(new XElement("item", new XElement("name", name)));
                el.Element(root.Name).Add(newEle);
            }
        }
    }
}
