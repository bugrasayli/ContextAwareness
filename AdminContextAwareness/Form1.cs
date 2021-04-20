using AdminContextAwareness.Models;
using AdminContextAwareness.Repository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AdminContextAwareness
{
    public partial class Form1 : Form
    {
        private CommunityRepository repo;
        private List<Community> communities;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            PositionRepository repo2 = new PositionRepository();
            repo = new CommunityRepository();
            Location locTest = new Location();
            locTest.x = 1.635;
            locTest.y = 0;
            locTest.W1 = 27;
            locTest.W2 = 14;
            locTest.W3 = 7;
            repo2.CalculatePosition(locTest);
            communities = repo.GetCommunities();
            foreach (var community in communities)
            {
                CommunityCmbx.DataSource = new BindingSource(communities, null);
                CommunityCmbx.DisplayMember = "Name";
                CommunityCmbx.ValueMember = "ID";
                //CommunityCmbx.Items.Add(community.Name);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UserProfile user = new UserProfile();
            user.Name = Name.Text;
            user.Surname = Surname.Text;
            user.Phone = Phone.Text;
            user.Area1 = FirstArea.Text;
            user.Area2 = SecondArea.Text;
            user.Area3 = ThirdArea.Text;
            Community value = (Community)CommunityCmbx.SelectedItem;
            user.CommunityID = value.ID;
            UserProfileRepository repo = new UserProfileRepository();
            repo.WriteUser(user);

        }
    }
}
