using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrMailMonitoring.Tests.Fixtures
{
    public class MyDbUtilsFixture : IDisposable
    {
        public string DbPath { get; set; }
        public Dictionary<string, string> ReceivedStrEmailsSamples { get; set; }


        public MyDbUtilsFixture()
        {
            this.DbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "_db", "StrMailMonitoring.db");
            this.ReceivedStrEmailsSamples = GenerateSampleRecords();
        }



        public Dictionary<string, string> GenerateSampleRecords()
        {
            Dictionary<string, string> rX = new Dictionary<string, string>()
            {
                ["Subject"] = "Shiraishi",
                ["FileName"] = "Maiyan.jpg",
                ["Email"] = "str_mail_monitoring.db",
                ["LotusUsername"] = "shiraishi",
                ["LastName"] = "shiraishi",
                ["FirstName"] = "mai",
                ["IsMandatory"] = "YES",
                ["IsAvailable"] = "YES",
                ["RecipientGroup"] = "Suika",
                ["Investigators"] = "ikuchanmomoko",
                ["Response"] = "APPROVED",
                ["ResponseDate"] = "s2020-10-07T00:00:00.000",
            };
            return rX;
        }



        public void Dispose()
        {
            Console.WriteLine();
        }
    }
}
