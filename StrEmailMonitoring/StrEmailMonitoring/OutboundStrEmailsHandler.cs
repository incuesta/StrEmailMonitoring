using StrEmailMonitoring.StrEmailMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrEmailMonitoring
{
    public class OutboundStrEmailsHandler
    {
        private string  DbPath { get; set; }
        private string  FileName { get; set; }
        private string  Subject { get; set; }
        private string  EmailUsernames { get; set; }
        private string SentDate { get; set; }
        private string Phase { get; set; }
        private const string TABLE_NAME = "OutboundStrEmails";

        public OutboundStrEmailsHandler(string dbPath)
        {
            this.DbPath = dbPath;
        }

        public OutboundStrEmailsHandler(string dbPath, string fileName = "", string subject = "", string emailUsernames = "", string sentDate = "")
        {
            this.DbPath = dbPath;
            this.FileName = fileName;
            this.Subject = subject;
            this.EmailUsernames = emailUsernames;
            this.SentDate = sentDate;
            this.Phase = GetPhase(emailUsernames);
        }

        /// <summary>
        /// Get the phase based on a given Username
        /// </summary>
        /// <returns></returns>
        public string GetPhase(string emailUsernames)
        {
            string resultX = string.Empty;
            string emailUsername = (emailUsernames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty).Trim();
            StrEmailsRecipientsHandler rHandler = new StrEmailsRecipientsHandler(this.DbPath);
            DataTable dtX = rHandler.LoadFromDbViaUsername(emailUsername);
            DataRow drX = dtX.AsEnumerable().FirstOrDefault();
            if (drX != null)
            {
                resultX = drX["RecipientGroup"].ToString().Trim();
            }
            return $"PHASE{resultX.Trim()}";
        }


        /// <summary>
        /// Add sent emails to DB
        /// </summary>
        public void InsertToDb()
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = this.FileName
            };
            MyDbUtils mduForCount = new MyDbUtils(this.DbPath);
            long resultCount = mduForCount.CountEntries(TABLE_NAME, criteriaX);

            if (resultCount < 1)
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["FileName"] = this.FileName,
                    ["Subject"] = this.Subject,
                    ["EmailUsernames"] = this.EmailUsernames,
                    ["SentDate"] = this.SentDate,
                    ["Phase"] = this.Phase
                };

                MyDbUtils mdu = new MyDbUtils(this.DbPath);
                mdu.CreateEntry(TABLE_NAME, recordsX);
            }
            else
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["Subject"] = this.Subject,
                    ["EmailUsernames"] = this.EmailUsernames,
                    ["SentDate"] = this.SentDate,
                    ["Phase"] = this.Phase
                };

                MyDbUtils mduForUpdate = new MyDbUtils(this.DbPath);
                mduForUpdate.UpdateEntry(TABLE_NAME, criteriaX, recordsX);
            }
        }


        /// <summary>
        /// Load an entry based from a given Phase
        /// </summary>
        /// <param name="targetPhase">PHASE1, PHASE2, PHASE3</param>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        public DataTable LoadPhaseXFromDb(string targetPhase = "PHASE1", int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> innerC = new Dictionary<string, string>()
            {
                ["Phase"] = targetPhase
            };

            Dictionary<string, string> outerC = new Dictionary<string, string>()
            {
                ["Phase"] = targetPhase
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntriesViaSubquery(TABLE_NAME, perBatch, currentBatch, targetColumn: "FileName", innerCriteria: innerC, outerCriteria: outerC);
        }


        /// <summary>
        /// Load an entry based from a given FileName
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        public DataTable LoadFileNameFromDb(string targetFile, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> innerC = new Dictionary<string, string>()
            {
                ["FileName"] = targetFile
            };

            Dictionary<string, string> outerC = new Dictionary<string, string>()
            {
                ["FileName"] = targetFile
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntriesViaSubquery(TABLE_NAME, perBatch, currentBatch, targetColumn: "FileName", innerCriteria: innerC, outerCriteria: outerC, outerOrderBy: "Phase");
        }

        /// <summary>
        /// Load an entry based from a given FileName
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        public DataTable LoadFileNameAndPhaseFromDb(string targetFile, string targetPhase, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> innerC = new Dictionary<string, string>()
            {
                ["FileName"] = targetFile,
                ["Phase"] = targetPhase
            };

            Dictionary<string, string> outerC = new Dictionary<string, string>()
            {
                ["FileName"] = targetFile,
                ["Phase"] = targetPhase
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntriesViaSubquery(TABLE_NAME, perBatch, currentBatch, targetColumn: "FileName", innerCriteria: innerC, outerCriteria: outerC, outerOrderBy: "Phase");
        }
    }
}
