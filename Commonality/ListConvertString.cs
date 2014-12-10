using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Commonality
{
    
    public class ListConvertString
    {
        //private static bool issucess = false;
        //public static bool IsSucess
        //{
        //    get { return issucess; }
        //    set { issucess = value; }
        //}
        /// <summary>
        /// 序列化 调用这个方法： string str = Serialize<Entity>(list); 
        /// </summary>
        /// <typeparam name="BusinessObject"></typeparam>
        /// <param name="GenericList"></param>
        /// <returns></returns>
        public static string Serialize<BusinessObject>(List<BusinessObject> GenericList, string strRoot, string strItem)
        {
            XmlDocument result = new XmlDocument();
            if (string.IsNullOrEmpty(strRoot))
                strRoot = "ROOT";
            if (string.IsNullOrEmpty(strItem))
                strItem = "ITEM";
            result.LoadXml("<" + strRoot.ToUpper() + "></" + strRoot.ToUpper() + ">");
            foreach (BusinessObject obj in GenericList)
            {
                XmlElement Item = result.CreateElement(strItem.ToUpper());
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj, null) != null)
                    {
                        XmlElement element = result.CreateElement(property.Name.ToUpper());
                        //element.SetAttribute("Type", property.PropertyType.Name.ToUpper());
                        element.InnerText = property.GetValue(obj, null).ToString();
                        Item.AppendChild(element);
                    }
                }
                result.DocumentElement.AppendChild(Item);
            }
            return result.InnerXml;
        }
        //foreach (KeyValuePair<string, string> kvp in myDic)

        /// <summary>
        /// Dictionary集合转换成string  （xml格式）序列化 调用这个方法： string str = Serialize<Entity>(list); 
        /// </summary>
        /// <typeparam name="BusinessObject"></typeparam>
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
            XmlElement Item = result.CreateElement(strItem.ToUpper());
            foreach (KeyValuePair<strKey, strValue> kvp in GenericList)
            {
                XmlElement element = result.CreateElement(kvp.Key.ToString().Trim());
                element.InnerText = kvp.Value.ToString();
                Item.AppendChild(element);
            }
            
            result.DocumentElement.AppendChild(Item);
            return result.InnerXml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="BusinessObject"></typeparam>
        /// <param name="GenericList"></param>
        /// <param name="strRoot"></param>
        /// <param name="strItem"></param>
        /// <param name="isCreateProperties">是否生成属性</param>
        /// <returns></returns>
        public static string Serialize<BusinessObject>(List<BusinessObject> GenericList, string strRoot, string strItem, bool isCreateProperties)
        {
            XmlDocument result = new XmlDocument();
            if (string.IsNullOrEmpty(strRoot))
                strRoot = "ROOT";
            if (string.IsNullOrEmpty(strItem))
                strItem = "ITEM";
            result.LoadXml("<?xml version='1.0' encoding='utf-8'?><" + strRoot.ToUpper() + "></" + strRoot.ToUpper() + ">".ToUpper());
            foreach (BusinessObject obj in GenericList)
            {
                XmlElement Item = result.CreateElement(strItem.ToUpper());
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj, null) != null)
                    {
                        XmlElement element = result.CreateElement(property.Name.ToUpper());
                        if (isCreateProperties)
                        {
                            element.SetAttribute("Type", property.PropertyType.Name.ToUpper());
                        }
                        element.InnerText = property.GetValue(obj, null).ToString();
                        Item.AppendChild(element);
                    }
                }
                result.DocumentElement.AppendChild(Item);
            }
            return result.InnerXml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="BusinessObject"></typeparam>
        /// <param name="GenericList"></param>
        /// <param name="isCreateProperties">是否生成属性</param>
        /// <returns></returns>
        //public static string Serialize<BusinessObject>(List<BusinessObject> GenericList,bool isCreateProperties)
        //{
        //    XmlDocument result = new XmlDocument();
        //    result.LoadXml("<Root></Root>");
        //    foreach (BusinessObject obj in GenericList)
        //    {
        //        XmlElement Item = result.CreateElement("Item");
        //        PropertyInfo[] properties = obj.GetType().GetProperties();
        //        foreach (PropertyInfo property in properties)
        //        {
        //            if (property.GetValue(obj, null) != null)
        //            {
        //                XmlElement element = result.CreateElement(property.Name);
        //                if (isCreateProperties)
        //                {
        //                    element.SetAttribute("Type", property.PropertyType.Name);
        //                }
        //                element.InnerText = property.GetValue(obj, null).ToString();
        //                Item.AppendChild(element);
        //            }
        //        }
        //        result.DocumentElement.AppendChild(Item);
        //    }
        //    return result.InnerXml;
        //}

        /// <summary>
        /// 反序列化 调用这个方法： List<Entity> list = Deserialize<Entity>(str); 
        /// </summary>
        /// <typeparam name="BusinessObject"></typeparam>
        /// <param name="XmlStr"></param>
        /// <returns></returns>
        public static List<BusinessObject> Deserialize<BusinessObject>(string XmlStr, string strRoot)
        {
            try
            {
                if (string.IsNullOrEmpty(strRoot))
                    strRoot = "ROOT";
                List<BusinessObject> result = new List<BusinessObject>();
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(XmlStr);
                foreach (XmlNode ItemNode in XmlDoc.GetElementsByTagName(strRoot).Item(0).ChildNodes)
                {
                    BusinessObject item = Activator.CreateInstance<BusinessObject>();
                    PropertyInfo[] properties = typeof(BusinessObject).GetProperties();
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
                            }
                        }
                    }

                    result.Add(item);
                }
                //IsSucess = true;
                //IsSucess = true;
                return result;
            }
            catch (Exception ex)
            {
                //IsSucess=false;
                //IsSucess = false;
                return null;
            }
        }



        public static List<BusinessObject> Deserialize<BusinessObject>(string XmlStr, string strRoot, ref bool IsSucess)
        {
            try
            {
                List<BusinessObject> result = new List<BusinessObject>();
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(XmlStr);
                foreach (XmlNode ItemNode in XmlDoc.GetElementsByTagName(strRoot).Item(0).ChildNodes)
                {
                    BusinessObject item = Activator.CreateInstance<BusinessObject>();
                    PropertyInfo[] properties = typeof(BusinessObject).GetProperties();
                    foreach (XmlNode propertyNode in ItemNode.ChildNodes)
                    {
                        string name = propertyNode.Name;
                        string type = propertyNode.Attributes["Type"].Value;
                        string value = propertyNode.InnerXml;
                        foreach (PropertyInfo property in properties)
                        {
                            if (name == property.Name)
                            {
                                property.SetValue(item, Convert.ChangeType(value, property.PropertyType), null);
                            }
                        }
                    }

                    result.Add(item);
                }
                //IsSucess = true;
                IsSucess = true;
                return result;
            }
            catch (Exception ex)
            {
                //IsSucess=false;
                IsSucess = false;
                return null;
            }
        }




    }
}
