using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StrEmailMonitoring.Tests
{
    public class InboundStrEmailsHandlerTests : IClassFixture<StrEmailsFixture>
    {
        public List<List<string>> InboundEmailsList { get; set; } = new List<List<string>>();
        public string DbPath { get; set; }
        public MyDbUtils DbUtilsX { get; set; }

        public InboundStrEmailsHandlerTests(StrEmailsFixture seFix)
        {
            this.DbPath = seFix.DbPath;
            this.InboundEmailsList = seFix.InboundEmailsList;
            this.DbUtilsX = seFix.DbUtils;
        }


        [Fact]
        public void InsertToDbTests_ShouldInsertInboundEmailsToDb()
        {
            foreach (List<string> emailX in InboundEmailsList)
            {
                string fileName = emailX[0];
                string subject = emailX[1];
                string emailUsername = emailX[2];
                string response = emailX[3];
                string responseDate = emailX[4];
                InboundStrEmailsHandler iSEHandler = new InboundStrEmailsHandler(this.DbPath, fileName, subject, emailUsername, response, responseDate);
                iSEHandler.InsertToDb();
            }
            MyDbUtils muForRead = new MyDbUtils(this.DbPath);
            long actualCount = muForRead.CountEntries("InboundStrEmails");
            long expectedCount = 12;
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetCurrentPhaseViaFileNameTests_ShouldReturnCorrectPhase()
        {
            InboundStrEmailsHandler iSEHandler = new InboundStrEmailsHandler(this.DbPath);
            string actualPhase = iSEHandler.GetCurrentPhaseViaFileName("file1.xlsx");
            string expectedPhase = "PHASE2";
            Assert.Equal(expectedPhase, actualPhase);
        }


        [Fact]
        public void IsApprovedTests_ShouldReturnApprovalStatus()
        {
            InboundStrEmailsHandler iSEHandler = new InboundStrEmailsHandler(this.DbPath);
            bool actualApproval = iSEHandler.IsApproved("file1.xlsx");
            Assert.True(actualApproval);
        }

        [Fact]
        public void IsDisapproved_ShouldReturnDisapprovalStatus()
        {
            InboundStrEmailsHandler iSEHandler = new InboundStrEmailsHandler(this.DbPath);
            bool actualApproval = iSEHandler.IsDisapproved("file1.xlsx");
            Assert.True(!actualApproval);
        }
    }
}
