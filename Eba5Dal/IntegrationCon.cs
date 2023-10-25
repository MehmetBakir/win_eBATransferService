// Decompiled with JetBrains decompiler
// Type: Eba5Dal.IntegrationCon
// Assembly: Eba5Dal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8D5B2CC1-1CC6-44D0-9729-E8114E5A2903
// Assembly location: C:\Users\User\Downloads\Eba5Dal.dll

using FOCryptoServices;
using System;
using System.Collections;
using System.Xml;

namespace Eba5Dal
{
    public class IntegrationCon
    {
        public static string Server = "EBA5TEST";
        public static string UserId = "admin";
        public static string Psw = "0000";
        public static string ConfigFile = "NacsoftConfig.xml";

        public static void Baglan(string konum)
        {
            XmlTextReader xmlTextReader = new XmlTextReader(konum + IntegrationCon.ConfigFile);
            while (xmlTextReader.Read())
            {
                if (xmlTextReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlTextReader.Name)
                    {
                        case "Server":
                            IntegrationCon.Server = Convert.ToString(xmlTextReader.ReadString());
                            break;
                        case "UserId":
                            IntegrationCon.UserId = Convert.ToString(xmlTextReader.ReadString());
                            break;
                        case "Psw":
                            IntegrationCon.Psw = Convert.ToString(xmlTextReader.ReadString());
                            break;
                    }
                }
            }
            xmlTextReader.Close();
        }

        public string ConStr() => "Data Source=E3000;User ID=EBAWORKFLOW5;Password=" + this.Cevirici("EBAWORKFLOW5");

        public string GetSettingsFile() => "D:\\Masters\\Config\\DBConfig.config";

        private string Cevirici(string strDbKey)
        {
            string str1 = "E3000";
            string str2 = "";
            string settingsFile = this.GetSettingsFile();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(settingsFile);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//DbConnectionSettings/ROW[ServerName=\"" + str1 + "\" and DbKey=\"" + strDbKey + "\"]");
            if (xmlNode != null)
            {
                ArrayList arrayList = new ArrayList();
                string str3 = xmlNode.SelectSingleNode("DbSchema").InnerText.ToString();
                str2 = xmlNode.SelectSingleNode("DbPwd").InnerText.ToString();
                arrayList.Add((object)str3);
                arrayList.Add((object)str2);
            }
            return ((StringDecryptor)new FOStringDecryptor((EncryptionAlgorithm)4)).Decrypt(str2, "simplicitysophistication").Trim().ToUpper();
        }
    }
}
