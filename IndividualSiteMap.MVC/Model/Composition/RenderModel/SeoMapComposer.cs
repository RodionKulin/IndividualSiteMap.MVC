using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace IndividualSiteMap.MVC
{
    public class SeoMapComposer
    {
        public void WriteXmlMapResponce(List<RenderItem> items, CompositionContext context)
        {
            Stream outputStream = context.HttpContext.Response.OutputStream;
            context.HttpContext.Response.ContentType = "text/xml";
            string hostUrl = context.HttpContext.Request.Url.Host;

            using (TextWriter textWriter = new StreamWriter(outputStream, System.Text.Encoding.UTF8))
            {
                WriteXmlMap(items, textWriter, hostUrl);
            }
        }

        protected void WriteXmlMap(List<RenderItem> items, TextWriter textWriter, string hostUrl)
        {
            XmlTextWriter writer = new XmlTextWriter(textWriter);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (RenderItem item in items)
            {
                WriteItem(writer, item, hostUrl);
            }

            WriteEndElement(writer);

            writer.WriteEndDocument();
        }

        protected void WriteItem(XmlTextWriter writer, RenderItem renderItem, string host)
        {
            SeoDescription description = renderItem.Node.Seo;
            
            if (description != null)
            {
                float priority = (int)description.Priority / 10f;

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://" + host + renderItem.Href);
                writer.WriteElementString("changefreq", description.UpdateFrequency.ToString());
                writer.WriteElementString("priority", priority.ToString());
                writer.WriteEndElement();

                if (renderItem.Children != null)
                {
                    foreach (RenderItem subItem in renderItem.Children)
                    {
                        WriteItem(writer, subItem, host);
                    }
                }
            }
        }

        protected void WriteEndElement(XmlTextWriter writer)
        {
            writer.WriteEndElement(); //urlset
        }

    }

}
