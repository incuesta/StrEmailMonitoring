using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrEmailMonitoring.StrEmailMonitoring
{
    public class StrEmailsRecipientsHandler
    {
        public DataTable StrEmailsRecipientsDT { get; set; }
        private string DbPath { get; set; }
        private const string TABLE_NAME = "StrEmailsRecipients";


        /// <summary>
        /// To be used with LoadFromDbViaUsername method
        /// </summary>
        /// <param name="dbPath"></param>
        public StrEmailsRecipientsHandler(string dbPath)
        {
            this.DbPath = dbPath;
        }


        /// <summary>
        /// To be used by Loadtotable
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="strEmailsRecipientsDT"></param>
        public StrEmailsRecipientsHandler(string dbPath, DataTable strEmailsRecipientsDT)
        {
            this.DbPath = dbPath;
            this.StrEmailsRecipientsDT = strEmailsRecipientsDT;
        }

        /// <summary>
        /// Loads the DT list from excel of recipients to DB
        /// </summary>
        public void LoadToTable()
        {
           

            if (this.StrEmailsRecipientsDT.Rows.Count > 0)
            {
                MyDbUtils mduForDelete = new MyDbUtils(this.DbPath);
                mduForDelete.DeleteAllEntries(TABLE_NAME);

                foreach (DataRow drX in this.StrEmailsRecipientsDT.AsEnumerable())
                {
                    Dictionary<string, string> recordsX = new Dictionary<string, string>()
                    {
                        ["Email"] = drX["Email"]?.ToString().Trim() ?? string.Empty,
                        ["EmailUsername"] = drX["EmailUsername"]?.ToString().Trim() ?? string.Empty,
                        ["LastName"] = drX["LastName"]?.ToString().Trim() ?? string.Empty,
                        ["FirstName"] = drX["FirstName"]?.ToString().Trim() ?? string.Empty,
                        ["IsMandatory"] = drX["IsMandatory"]?.ToString().Trim() ?? string.Empty,
                        ["IsAvailable"] = drX["IsAvailable"]?.ToString().Trim() ?? string.Empty,
                        ["RecipientGroup"] = drX["RecipientGroup"]?.ToString().Trim() ?? string.Empty,
                        ["Investigators"] = drX["Investigators"]?.ToString().Trim() ?? string.Empty
                    };
                    MyDbUtils mdu = new MyDbUtils(this.DbPath);
                    mdu.CreateEntry(TABLE_NAME, recordsX);
                }
            }
            else
            {
                Console.WriteLine($"[StrEmailsRecipientsHandler] DataTable is empty");
                
            }
        }


        /// <summary>
        /// Load an entry based from a given Username
        /// </summary>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        public DataTable LoadFromDbViaUsername(string emailUsername, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["EmailUsername"] = emailUsername
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntries(tableName: TABLE_NAME, orderBy: "EmailUsername", perBatch: perBatch, currentBatch: currentBatch, criteria: criteriaX);
        }

    }
}
