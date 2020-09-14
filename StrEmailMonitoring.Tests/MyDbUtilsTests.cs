using StrMailMonitoring.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;

namespace StrEmailMonitoring.Tests
{
    //public class MyDbUtilsTests : IClassFixture<MyDbUtilsFixture>, IDisposable
    //{
        //public Dictionary<string,string> ReceivedStrEmailsSamples  { get; set; }
        //public string  DbPath { get; set; }
        //public string receivedStrEmailsTableName { get; set; } = "StrEmails";

        //public MyDbUtilsTests(MyDbUtilsFixture mdFix)
        //{
        //    this.ReceivedStrEmailsSamples = mdFix.ReceivedStrEmailsSamples;
        //    this.DbPath = mdFix.DbPath;
        //}

        //[Fact]
        //public void InsertDbEntriesTests_ShoulInsertEntriesToTable()
        //{

        //    MyDbUtils mdu = new MyDbUtils(this.DbPath);
        //    mdu.CreateEntry(tableName: receivedStrEmailsTableName, this.ReceivedStrEmailsSamples);
        //    Assert.Equal(1, mdu.CountEntries(receivedStrEmailsTableName));
        //}

        //public void Dispose()
        //{
        //    MyDbUtils mdu = new MyDbUtils(this.DbPath);
        //    mdu.DeleteAllEntries(receivedStrEmailsTableName);
        //}
    //}
}
