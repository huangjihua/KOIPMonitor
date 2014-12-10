using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Commonality
{
    public class DictionaryConvertString
    {
        /// <summary>
        /// string T_MenumList = CreateXmlByDictionary<string, Mode_T_Menum>(MusicDAL._publicvariable.T_MenumList, "Root", "Item");
        /// </summary>
        /// <typeparam name="strKey"></typeparam>
        /// <typeparam name="strValue"></typeparam>
        /// <param name="GenericList"></param>
        /// <param name="strRoot"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static string CreateXmlByDictionary<strKey, strValue>(Dictionary<strKey, strValue> GenericList, string strRoot, string strItem)
        {
            XmlDocument result = new XmlDocument();
            if (string.IsNullOrEmpty(strRoot))
                strRoot = "ROOT";
            if (string.IsNullOrEmpty(strItem))
                strItem = "ITEM";
            result.LoadXml("<?xml version='1.0' encoding='utf-8'?><" + strRoot.ToUpper() + "></" + strRoot.ToUpper() + ">".ToUpper());
            //XmlElement Item = result.CreateElement(strItem);
            foreach (KeyValuePair<strKey, strValue> kvp in GenericList)
            {
                XmlElement Item = result.CreateElement(strItem.ToUpper());
                PropertyInfo[] properties = kvp.Value.GetType().GetProperties();

                //MemberInfo[] fieldInfo = kvp.GetType().GetMembers();

                //PropertyInfo[] properties1 = properties[1].GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    //XmlElement element1 = result.CreateElement(property.Name);
                    if (property.GetValue(kvp.Value, null) != null)
                    {
                        XmlElement element = result.CreateElement(property.Name.ToUpper());
                        //element.InnerText = property.GetValue(kvp.Value, null).ToString();
                        element.InnerText = property.GetValue(kvp.Value, null).ToString();
                        //element.InnerText = kvp.Value.ToString();
                        Item.AppendChild(element);
                    }

                }
                //XmlElement element = result.CreateElement(kvp.Value.ToString().Trim());

                result.DocumentElement.AppendChild(Item);
            }


            return result.InnerXml;
        }

        /// <summary>
        ///Dictionary<string, Mode_T_Menum> rrr = SQLHelper.DictionaryConvertString.Deserialize<string, Mode_T_Menum>(T_MenumList, "Root");
        /// </summary>
        /// <typeparam name="aa"></typeparam>
        /// <typeparam name="strValue"></typeparam>
        /// <param name="XmlStr"></param>
        /// <param name="strRoot"></param>
        /// <returns></returns>
        public static Dictionary<string, strValue> Deserialize<aa, strValue>(string XmlStr, string strRoot)
        {
            try
            {
                //List<BusinessObject> result = new List<BusinessObject>();
                if (string.IsNullOrEmpty(strRoot))
                    strRoot = "ROOT";
                Dictionary<string, strValue> result = new Dictionary<string, strValue>();
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(XmlStr);
                foreach (XmlNode ItemNode in XmlDoc.GetElementsByTagName(strRoot).Item(0).ChildNodes)
                {
                    strValue item = Activator.CreateInstance<strValue>();
                    //strKey strkey = Activator.CreateInstance<strKey>();
                    PropertyInfo[] properties = typeof(strValue).GetProperties();
                    int i = 1;
                    string strkey = string.Empty;
                    //typeof(strKey) aa;
                    foreach (XmlNode propertyNode in ItemNode.ChildNodes)
                    {
                        string name = propertyNode.Name;
                        //string type = propertyNode.Attributes["Type"].Value;
                        string value = propertyNode.InnerXml;
                        foreach (PropertyInfo property in properties)
                        {
                            if (name == property.Name)
                            {
                                property.SetValue(item, Convert.ChangeType(value, property.PropertyType), null);
                                if (i == 1)
                                {
                                    strkey = Convert.ChangeType(value, property.PropertyType).ToString();
                                    //property.SetValue(strkey, Convert.ChangeType(value, property.PropertyType), null);
                                }
                            }
                        }
                        i++;
                    }
                    result.Add(strkey, item);
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}