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
    public class InboundStrEmailsHandler
    {
        private string  DbPath { get; set; }
        private string  FileName { get; set; }
        private string  Subject { get; set; }
        private string  EmailUsername { get; set; }
        private string  Response { get; set; }
        private string  ResponseDate { get; set; }
        private string  Phase { get; set; }
        private const string TABLE_NAME = "InboundStrEmails";


        public InboundStrEmailsHandler(string dbPath, string fileName = "", string subject = "", string emailUsername = "", string response = "NONE", string responseDate = "")
        {
            this.DbPath = dbPath;
            this.FileName = fileName;
            this.Subject = subject;
            this.EmailUsername = emailUsername;
            this.Response = response;
            this.ResponseDate = responseDate;
            this.Phase = GetPhase(emailUsername);
        }


        /// <summary>
        /// Get the phase based on a given Username
        /// </summary>
        /// <returns></returns>
        public string GetPhase(string emailUsername)
        {
            string resultX = string.Empty;
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
        /// Insert entry if non existent otherwise update entry
        /// </summary>
        public void InsertToDb()
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = this.FileName,
                ["EmailUsername"] = this.EmailUsername,
            };
            MyDbUtils mduForCount = new MyDbUtils(this.DbPath);
            long resultCount = mduForCount.CountEntries(TABLE_NAME, criteriaX);

            if (resultCount < 1)
            {
                Dictionary<string, string> recordsX = new Dictionary<string, string>()
                {
                    ["FileName"] = this.FileName,
                    ["Subject"] = this.Subject,
                    ["EmailUsername"] = this.EmailUsername,
                    ["Response"] = this.Response,
                    ["ResponseDate"] = this.ResponseDate,
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
                    ["Response"] = this.Response,
                    ["ResponseDate"] = this.ResponseDate,
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

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntriesViaSubquery(TABLE_NAME, perBatch, currentBatch, targetColumn: "FileName", innerCriteria: innerC);
        }


        /// <summary>
        /// Load an entry based from a given FileName and Username
        /// </summary>
        /// <param name="targetPhase">PHASE1, PHASE2, PHASE3</param>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        public DataTable LoadFromDbViaFileNameAndUsername(string fileName, string emailUsername, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName,
                ["EmailUsername"] = emailUsername
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntries( tableName: TABLE_NAME, orderBy: "Phase", perBatch: perBatch, currentBatch: currentBatch, criteria: criteriaX);
        }

        /// <summary>
        /// Load from db using FileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        /// <returns></returns>
        public DataTable LoadFromDbViaFileName(string fileName, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName            
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            return mdu.LoadEntries(tableName: TABLE_NAME, orderBy: "Phase", perBatch: perBatch, currentBatch: currentBatch, criteria: criteriaX);
        }

        /// <summary>
        /// Get Current Phase of a File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetCurrentPhaseViaFileName(string fileName)
        {
            string phaseString = "";
            DataTable dtX = LoadFromDbViaFileName(fileName);
            phaseString = (dtX.AsEnumerable().OrderBy(drX => drX["Phase"])?.LastOrDefault())["Phase"]?.ToString() ?? string.Empty;
            return phaseString;
        }


        /// <summary>
        /// Determine approval
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsApproved(string fileName)
        {
            string phaseX = GetCurrentPhaseViaFileName(fileName);
            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            Dictionary<string, string> countCritX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName,
                ["Phase"] = phaseX,
                ["Response"] = "APPROVED"
            };
            long resultCt = mdu.CountEntries(TABLE_NAME, countCritX);
            MessageBox.Show(resultCt.ToString());
            return resultCt >= 2;
        }
    }
}
