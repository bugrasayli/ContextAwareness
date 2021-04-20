using AdminContextAwareness.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AdminContextAwareness.Repository
{
    public class UserProfileRepository
    {
        public UserProfileRepository()
        {

        }
        public void WriteUser(UserProfile user)
        {
            string Url = @"C:\Users\bugra\Dropbox\My PC (DESKTOP-4B177VT)\Desktop\Project\AdminContextAwareness\AdminContextAwareness\Database\UserProfile.xml";
            if (!File.Exists(Url))
            {
                using (XmlWriter writer = XmlWriter.Create(Url))
                {
                    writer.WriteStartElement("UserProfile");
                    writer.WriteStartElement("User");
                    writer.WriteElementString("Name", user.Name);
                    writer.WriteElementString("Surname", user.Surname);
                    writer.WriteElementString("Phone", user.Phone);
                    writer.WriteElementString("Area1", user.Area1);
                    writer.WriteElementString("Area2", user.Area2);
                    writer.WriteElementString("Area3", user.Area3);
                    writer.WriteElementString("CommunityID", user.CommunityID.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load(Url);
                XElement root = xDocument.Element("UserProfile");
                IEnumerable<XElement> rows = root.Descendants("User");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                    new XElement("User",
                    new XElement("Name", user.Name),
                    new XElement("Surname", user.Surname),
                    new XElement("Phone", user.Phone),
                    new XElement("Area1", user.Area1),
                    new XElement("Area2", user.Area2),
                    new XElement("Area3", user.Area3),
                    new XElement("CommunityID", user.CommunityID.ToString())
                    ));
                xDocument.Save(Url);

            }
        }
    }
}
