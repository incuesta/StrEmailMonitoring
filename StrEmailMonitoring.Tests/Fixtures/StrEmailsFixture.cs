using MyXL;
using StrEmailMonitoring.StrEmailMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrEmailMonitoring.Tests
{
    public class StrEmailsFixture : IDisposable
    {
        private string RecipientsFile { get; set; }
        public string DbPath { get; set; }
        public List<List<string>> OutboundEmailsList { get; set; }
        public List<List<string>> InboundEmailsList { get; set; }
        public MyDbUtils DbUtils { get; set; }

        public StrEmailsFixture()
        {
            this.RecipientsFile = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Inputs", "str_emails_recipients.xlsx");
            this.DbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "_Db", "StrEmailMonitoring.db");
            LoadRecipientsFromFile();// load to db
            this.DbUtils = new MyDbUtils(this.DbPath);
            this.OutboundEmailsList = SetOutboundEmailsList();
            this.InboundEmailsList = SetInboundEmailsList();
        }


        private void LoadRecipientsFromFile() 
        {
            DataTable recipientsDT = MyXLMain.ReadRange(this.RecipientsFile, "data");
            StrEmailsRecipientsHandler seh = new StrEmailsRecipientsHandler(this.DbPath, recipientsDT);
            seh.LoadToTable();
        }

        private List<List<string>> SetOutboundEmailsList()
        {
            List<List<string>> listX = new List<List<string>>();
            List<string> list1 = new List<string>() { 
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Raoul Reniedo/Banco_de_Oro",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list1);

            List<string> list2 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Gembeth G Basilgo/BDO",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list2);

            List<string> list3 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Edna C Agajan/Banco_de_Oro, Raoul Reniedo/Banco_de_Oro, Gembeth G Basilgo/BDO",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list3);

            List<string> list4 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE1)",
                "Edna C Agajan/Banco_de_Oro, Raoul Reniedo/Banco_de_Oro, Gembeth G Basilgo/BDO",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list4);

            List<string> list5 = new List<string>() {
                "file3.xlsx",
                "STR Committee Approval [Filname3.docx] (PHASE2)",
                "Federico P Tancongco/BDO,Alvin C Go/Banco_de_Oro,Ricky Martin/Banco_de_Oro,Ma. Corazon A Mallillin/Banco_de_Oro,Jeanette S. Javellana/Banco_de_Oro",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list5);

            return listX;
        }


        private List<List<string>> SetInboundEmailsList()
        {
            List<List<string>> listX = new List<List<string>>();
            List<string> list1 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Raoul Reniedo/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list1);

            List<string> list2 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Edna C Agajan/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list2);


            List<string> list3 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Gembeth G Basilgo/BDO",
                "REJECTED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list3);

            List<string> list4 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE2)",
                "Federico P Tancongco/BDO",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list4);

            List<string> list5 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE2)",
                "Alvin C Go/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list5);

            List<string> list6 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE2)",
                "Ricky Martin/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list6);

            List<string> list7 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE2)",
                "Ma. Corazon A Mallillin/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list7);

            List<string> list8 = new List<string>() {
                "file2.xlsx",
                "STR Committee Approval [Filname2.docx] (PHASE2)",
                "Jeanette S. Javellana/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list8);


            List<string> list9 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Jeanette S. Javellana/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list9);

            List<string> list10 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Ma. Corazon A Mallillin/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list10);


            List<string> list11 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Ricky Martin/Banco_de_Oro",
                "REJECTED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list11);

            List<string> list12 = new List<string>() {
                "file1.xlsx",
                "STR Committee Approval [Filname1.docx] (PHASE1)",
                "Alvin C Go/Banco_de_Oro",
                "APPROVED",
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")
            };
            listX.Add(list12);



            return listX;

            
        }


        public void Dispose()
        {
            this.DbUtils = null;
            this.OutboundEmailsList = null;
        }
    }


}
