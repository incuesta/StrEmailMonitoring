using MyXL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;

namespace StrEmailMonitoring.Tests
{
    public class OutboundStrEmailsHandlerTests : IClassFixture<StrEmailsFixture>, IDisposable
    {
        public List<List<string>> OutboundEmailsList { get; set; } = new List<List<string>>();
        public string DbPath { get; set; }
        public MyDbUtils DbUtilsX { get; set; }

        public OutboundStrEmailsHandlerTests(StrEmailsFixture seFix)
        {
            this.OutboundEmailsList = seFix.OutboundEmailsList;
            this.DbPath = seFix.DbPath;
            this.DbUtilsX = seFix.DbUtils;
        }

        [Fact]
        public void InsertToDbTest_ShouldInsertEntriesToOuboundStrEmails()
        {
            foreach(List<string> entryX in this.OutboundEmailsList)
            {
                string fileName = entryX[0];
                string subject = entryX[1];
                string emailUsername = entryX[2];
                string sentDate = entryX[3];
                OutboundStrEmailsHandler outH = new OutboundStrEmailsHandler(this.DbPath, fileName, subject, emailUsername, sentDate);
                outH.InsertToDb();
            }
            MyDbUtils muForRead = new MyDbUtils(this.DbPath);
            long actualCount = muForRead.CountEntries("OutboundStrEmails");
            long expectedCount = 3;
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void LoadPhaseXFromDb_ShouldLoadEntriesViaGivenPhase()
        {
            foreach (List<string> entryX in this.OutboundEmailsList)
            {
                string fileName = entryX[0];
                string subject = entryX[1];
                string emailUsername = entryX[2];
                string sentDate = entryX[3];
                OutboundStrEmailsHandler outH = new OutboundStrEmailsHandler(this.DbPath, fileName, subject, emailUsername, sentDate);
                outH.InsertToDb();
            }
            OutboundStrEmailsHandler outH2 = new OutboundStrEmailsHandler(this.DbPath);
            DataTable dtX = outH2.LoadPhaseXFromDb("PHASE1");
            long expectedCount = 2;
            long actualCount = (long) dtX.Rows.Count;
            Assert.Equal(expectedCount, actualCount);
        }


        public void Dispose()
        {
            //this.DbUtilsX.DeleteAllEntries("OutboundStrEmails");
        }
    }
}
