using AdminContextAwareness.Models;
using System;
using System.Collections.Generic;
using System.Xml;

namespace AdminContextAwareness.Repository
{
    public class CommunityRepository
    {
        List<Community> communities;
        public CommunityRepository()
        {
            communities = new List<Community>();
        }
        public List<Community> GetCommunities()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\bugra\Dropbox\My PC (DESKTOP-4B177VT)\Desktop\Project\AdminContextAwareness\AdminContextAwareness\Database\Community.xml");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Community com = new Community();
                int a = 0;
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (a == 0){
                        com.ID = Convert.ToInt32(node2.InnerText);
                    }
                    if (a != 0){
                        com.Name = node2.InnerText;
                    }
                    a++;
                }
                communities.Add(com);
            }
            return communities;
        }
    }
}

