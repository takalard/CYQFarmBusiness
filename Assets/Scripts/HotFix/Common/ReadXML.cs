using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ReadXML
    {
        XmlNodeList xmlNodeList;
        XmlDocument xmlDoc;
        public ReadXML(string path, int ChildNode = -1)
        {
            //TextAsset xml = Resources.Load<TextAsset>(path); //Read File xml
            var operationHandler = YooAsset.YooAssets.LoadAssetSync<TextAsset>(path);
            xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader((operationHandler.AssetObject as TextAsset).text));
            xmlNodeList = xmlDoc.DocumentElement.ChildNodes; // ----> Read all childNode in file
            if (ChildNode != -1)
            {
                xmlNodeList = xmlNodeList.Item(ChildNode).ChildNodes;// ----> read all childNode of one childNode
            }
        }

        public XmlNode getDataByValue(string key, string val)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                if (node.Attributes[key].Value.Equals(val)) return node;
            }
            return null;
        }
        public XmlNode getDataByIndex(int index)
        {
            if (index >= xmlNodeList.Count)
                return null;
            else return xmlNodeList[index];
        }
        public XmlNode getDataByName(string name)
        {
            return xmlDoc.SelectSingleNode("Missions").SelectSingleNode(name);
        }

    }
}
