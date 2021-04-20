using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminContextAwareness.Models
{
    public class UserProfile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Area1 { get; set; }
        public string Area2 { get; set; }
        public string Area3 { get; set; }
        public int CommunityID { get; set; }
    }
}
