using AdminContextAwareness.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AdminContextAwareness.Repository
{
    public class PositionRepository
    {
        List<Location> locations;
        public PositionRepository()
        {
            locations = new List<Location>();
            getExcelFile();
        }
        public void getExcelFile()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\bugra\Dropbox\My PC (DESKTOP-4B177VT)\Desktop\Project\AdminContextAwareness\AdminContextAwareness\LocationTest\SchoolDataset2.xlsx");
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            for (int i = 1; i <= rowCount; i++)
            {
                Location loc = new Location();

                loc.x = xlRange.Cells[i, 1].Value2;
                loc.y = xlRange.Cells[i, 2].Value2;
                loc.W1 = xlRange.Cells[i, 3].Value2;
                loc.W2 = xlRange.Cells[i, 4].Value2;
                loc.W3 = xlRange.Cells[i, 5].Value2;
                loc.ReferancePointID = i;
                locations.Add(loc);
            }

        }

        public Location CalculatePosition(Location location)
        {
            List<Distance> distances = new List<Distance>();
            foreach (var loc in locations)
            {
                Distance dis = new Distance();
                double RSSIA_Distance = (location.W1 - loc.W1) * (location.W1 - loc.W1);
                double RSSIB_Distance = (location.W2 - loc.W2) * (location.W2 - loc.W2);
                double RSSIC_Distance = (location.W3 - loc.W3) * (location.W3 - loc.W3);
                double distance = Math.Sqrt(RSSIA_Distance + RSSIB_Distance + RSSIC_Distance);
                dis.ID = loc.ReferancePointID;
                dis.RssiDistance = distance;
                distances.Add(dis);
            }
            distances = distances.OrderByDescending(x => x.RssiDistance).Reverse().ToList();
            List<Location> knn = new List<Location>();
            distances = distances.Take(3).ToList();
            foreach (var item in distances)
            {
                knn.Add(locations.Where(x => x.ReferancePointID == item.ID).FirstOrDefault());
            }
            double SumX = 0;
            double SumY = 0;
            Location RealTimeLocation = new Location();
            foreach (var item in knn)
            {
                SumX += item.x;
                SumY += item.y;
            }
            RealTimeLocation.x = SumX / 3;
            RealTimeLocation.y = SumY / 3;
            RealTimeLocation.W1 = location.W1;
            RealTimeLocation.W2 = location.W2;
            RealTimeLocation.W3 = location.W3;
            WriteRawData(RealTimeLocation);
            return RealTimeLocation;
        }

        public void WriteRawData(Location loc)
        {
            string Url = @"C:\Users\bugra\Dropbox\My PC (DESKTOP-4B177VT)\Desktop\Project\AdminContextAwareness\AdminContextAwareness\SensorDatabase\LocationSensorRaw.xml";
            if (!File.Exists(Url))
            {
                using (XmlWriter writer = XmlWriter.Create(Url))
                {
                    writer.WriteStartElement("LocationSensor");
                    writer.WriteStartElement("Location");
                    writer.WriteElementString("WifiBox1", loc.W1.ToString());
                    writer.WriteElementString("WifiBox2", loc.W2.ToString());
                    writer.WriteElementString("WifiBox3", loc.W3.ToString());
                    writer.WriteElementString("X", loc.x.ToString());
                    writer.WriteElementString("Y", loc.y.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load(Url);
                XElement root = xDocument.Element("LocationSensor");
                IEnumerable<XElement> rows = root.Descendants("Location");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                    new XElement("Location",
                    new XElement("WifiBox1", loc.W1.ToString()),
                    new XElement("WifiBox2", loc.W2.ToString()),
                    new XElement("WifiBox3", loc.W3.ToString()),
                    new XElement("LocationX", loc.x.ToString()),
                    new XElement("LocationY", loc.y.ToString())));
                xDocument.Save(Url);

            }
        }
    }
    public class Distance
    {
        public int ID { get; set; }
        public double RssiDistance { get; set; }
    }

}
