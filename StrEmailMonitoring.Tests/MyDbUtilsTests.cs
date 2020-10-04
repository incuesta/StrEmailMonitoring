using StrMailMonitoring.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;
using Xunit.Sdk;

namespace StrEmailMonitoring.Tests
{
    public class MyDbUtilsTests : IClassFixture<MyDbUtilsFixture>, IDisposable
    {
        public List<List<string>> InboundEmailsList { get; set; }
        public List<List<string>> OutboundEmailsList { get; set; }
        public string DbPath { get; set; }

        public MyDbUtilsTests(MyDbUtilsFixture mdFix)
        {
            this.InboundEmailsList = mdFix.InboundEmailsList;
            this.DbPath = mdFix.DbPath;
            this.OutboundEmailsList = mdFix.OutboundEmailsList;
        }

        [Fact]
        public void InsertUpdateToDbTests_ShouldInsertUpdateInboundEmailsToDb()
        {
            foreach (List<string> emailX in InboundEmailsList)
            {
                string fileName = emailX[0];
                string subject = emailX[1];
                string emailUsername = emailX[2];
                string response = emailX[3];
                string responseDate = emailX[4];
                string phase = emailX[5];
                CreateEntryInboundStrEmails(fileName, emailUsername, "InboundStrEmails", subject, response, responseDate, phase);
            }
            foreach (List<string> emailX in OutboundEmailsList)
            {
                string fileName = emailX[0];
                string subject = emailX[1];
                string emailUsername = emailX[2];
                string sentDate = emailX[3];
                string phase = emailX[4];
                CreateEntryOutboundStrEmails(fileName, emailUsername, "OutboundStrEmails", subject, sentDate, phase);
            }
            MyDbUtils muForRead = new MyDbUtils(this.DbPath);
            long actualCount = muForRead.CountEntries("InboundStrEmails");
            long expectedCount = 11;
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void CountEntriesViaSubqueryTests_ShouldReturnCorrectCount()
        {
            Dictionary<string, string> inCritX = new Dictionary<string, string>()
            {
                ["Response"] = "REJECTED",
                ["Phase"] = "PHASE1"
            };
            Dictionary<string, string> critX = new Dictionary<string, string>()
            {
                ["Response"] = "REJECTED",
            };
            MyDbUtils muForRead = new MyDbUtils(this.DbPath);
            long expectedCount = 3;
            long actualCount = muForRead.CountEntriesViaSubquery("InboundStrEmails", criteria: critX, innerCriteria: inCritX, innerColumn: "FileName", innerTable: "OutboundStrEmails");
            Assert.Equal(expectedCount, actualCount);
        }

        private void CreateEntryInboundStrEmails(string FileName, string EmailUsername, string TABLE_NAME, string Subject, string Response, string ResponseDate, string Phase)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = FileName,
                ["EmailUsername"] = EmailUsername,
            };
            MyDbUtils mduForCount = new MyDbUtils(this.DbPath);
            long resultCount = mduForCount.CountEntries(TABLE_NAME, criteriaX);

            if (resultCount < 1)
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["FileName"] = FileName,
                    ["Subject"] = Subject,
                    ["EmailUsername"] = EmailUsername,
                    ["Response"] = Response,
                    ["ResponseDate"] = ResponseDate,
                    ["Phase"] = Phase
                };

                MyDbUtils mdu = new MyDbUtils(this.DbPath);
                mdu.CreateEntry(TABLE_NAME, recordsX);
            }
            else
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["Subject"] = Subject,
                    ["Response"] = Response,
                    ["ResponseDate"] = ResponseDate,
                    ["Phase"] = Phase
                };

                MyDbUtils mduForUpdate = new MyDbUtils(this.DbPath);
                mduForUpdate.UpdateEntry(TABLE_NAME, criteriaX, recordsX);
            }
        }

        private void CreateEntryOutboundStrEmails(string FileName, string EmailUsernames, string TABLE_NAME, string Subject, string SentDate, string Phase)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = FileName
            };
            MyDbUtils mduForCount = new MyDbUtils(DbPath);
            long resultCount = mduForCount.CountEntries(TABLE_NAME, criteriaX);

            if (resultCount < 1)
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["FileName"] = FileName,
                    ["Subject"] = Subject,
                    ["EmailUsernames"] = EmailUsernames,
                    ["SentDate"] = SentDate,
                    ["Phase"] = Phase
                };

                MyDbUtils mdu = new MyDbUtils(DbPath);
                mdu.CreateEntry(TABLE_NAME, recordsX);
            }
            else
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["Subject"] = Subject,
                    ["EmailUsernames"] = EmailUsernames,
                    ["SentDate"] = SentDate,
                    ["Phase"] = Phase
                };

                MyDbUtils mduForUpdate = new MyDbUtils(DbPath);
                mduForUpdate.UpdateEntry(TABLE_NAME, criteriaX, recordsX);
            }
        }


        public void Dispose()
        {
           
        }
    }
}
