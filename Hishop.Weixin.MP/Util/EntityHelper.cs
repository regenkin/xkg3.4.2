using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Util
{
    // Token: 0x02000038 RID: 56
    public static class EntityHelper
    {
        // Token: 0x06000139 RID: 313 RVA: 0x00005A5C File Offset: 0x00003C5C
        public static XDocument ConvertEntityToXml<T>(T entity) where T : class, new()
        {
            T arg_17_0;
            if ((arg_17_0 = entity) == null)
            {
                arg_17_0 = Activator.CreateInstance<T>();
            }
            entity = arg_17_0;
            XDocument xDocument = new XDocument();
            xDocument.Add(new XElement("xml"));
            XElement root = xDocument.Root;
            List<string> @object = new string[]
            {
                "ToUserName",
                "FromUserName",
                "CreateTime",
                "MsgType",
                "Content",
                "ArticleCount",
                "Articles",
                "FuncFlag",
                "Title ",
                "Description ",
                "PicUrl",
                "Url"
            }.ToList<string>();
            Func<string, int> orderByPropName = new Func<string, int>(@object.IndexOf);
            List<PropertyInfo> list = (from p in entity.GetType().GetProperties()
                                       orderby orderByPropName(p.Name)
                                       select p).ToList<PropertyInfo>();
            foreach (PropertyInfo current in list)
            {
                string name = current.Name;
                if (name == "Articles")
                {
                    XElement xElement = new XElement("Articles");
                    List<Article> list2 = current.GetValue(entity, null) as List<Article>;
                    foreach (Article current2 in list2)
                    {
                        IEnumerable<XElement> content = EntityHelper.ConvertEntityToXml<Article>(current2).Root.Elements();
                        xElement.Add(new XElement("item", content));
                    }
                    root.Add(xElement);
                }
                else
                {
                    string name2 = current.PropertyType.Name;
                    if (name2 == null)
                    {
                        goto IL_353;
                    }
                    if (!(name2 == "String"))
                    {
                        if (!(name2 == "DateTime"))
                        {
                            if (!(name2 == "Boolean"))
                            {
                                if (!(name2 == "ResponseMsgType"))
                                {
                                    if (!(name2 == "Article"))
                                    {
                                        goto IL_353;
                                    }
                                    root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
                                }
                                else
                                {
                                    root.Add(new XElement(name, current.GetValue(entity, null).ToString().ToLower()));
                                }
                            }
                            else
                            {
                                if (!(name == "FuncFlag"))
                                {
                                    goto IL_353;
                                }
                                root.Add(new XElement(name, ((bool)current.GetValue(entity, null)) ? "1" : "0"));
                            }
                        }
                        else
                        {
                            root.Add(new XElement(name, ((DateTime)current.GetValue(entity, null)).Ticks));
                        }
                    }
                    else
                    {
                        root.Add(new XElement(name, new XCData((current.GetValue(entity, null) as string) ?? "")));
                    }
                    continue;
                    IL_353:
                    root.Add(new XElement(name, current.GetValue(entity, null)));
                }
            }
            return xDocument;
        }

        // Token: 0x06000138 RID: 312 RVA: 0x000057AC File Offset: 0x000039AC
        public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : AbstractRequest, new()
        {
            if (entity == null)
            {
                entity = Activator.CreateInstance<T>();
            }
            XElement root = doc.Root;
            PropertyInfo[] properties = entity.GetType().GetProperties();
            string name = "";
            foreach (PropertyInfo info in properties)
            {
                name = info.Name;
                try
                {
                    if (root.Element(name) == null)
                    {
                        continue;
                    }
                    switch (info.PropertyType.Name)
                    {
                        case "DateTime":
                            {
                                info.SetValue(entity, new DateTime(long.Parse(root.Element(name).Value)), null);
                                continue;
                            }
                        case "Boolean":
                            {
                                if (name != "FuncFlag")
                                {
                                    break;
                                }
                                info.SetValue(entity, root.Element(name).Value == "1", null);
                                continue;
                            }
                        case "Int64":
                            {
                                info.SetValue(entity, long.Parse(root.Element(name).Value), null);
                                continue;
                            }
                        case "RequestEventType":
                            {
                                info.SetValue(entity, EventTypeHelper.GetEventType(root.Element(name).Value), null);
                                continue;
                            }
                        case "RequestMsgType":
                            {
                                info.SetValue(entity, MsgTypeHelper.GetMsgType(root.Element(name).Value), null);
                                continue;
                            }
                        case "Single":
                            {
                                float result = 0f;
                                float.TryParse(root.Element(name).Value, out result);
                                info.SetValue(entity, result, null);
                                continue;
                            }
                        default:
                            info.SetValue(entity, root.Element(name).Value, null);
                            continue;
                    }
                    info.SetValue(entity, root.Element(name).Value, null);
                    continue;
                }
                catch
                {
                }
            }
        }
    }
}
