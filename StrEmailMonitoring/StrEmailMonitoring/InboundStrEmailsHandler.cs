﻿using StrEmailMonitoring.StrEmailMonitoring;
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
        private string  Inv { get; set; }
        private string  Msg { get; set; }
        private List<DataRow>  StrRecipientsDrList { get; set; }
        private const string TABLE_NAME = "InboundStrEmails";


        public InboundStrEmailsHandler(string dbPath, string fileName = "", string subject = "", string emailUsername = "", string response = "NONE", string responseDate = "", string inv = "", string msg = "")
        {
            this.DbPath = dbPath;
            this.FileName = fileName;
            this.Subject = subject;
            this.EmailUsername = emailUsername;
            this.Response = response;
            this.ResponseDate = responseDate;
            this.Phase = GetPhase(emailUsername);
            this.Inv = inv;
            this.Msg = msg;
        }

        public InboundStrEmailsHandler(string dbPath, DataTable strRecipientsDT)
        {
            this.DbPath = dbPath;
            this.StrRecipientsDrList = strRecipientsDT.AsEnumerable().ToList();
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
                    ["Phase"] = this.Phase,
                    ["Inv"] = this.Inv,
                    ["Message"] = this.Msg
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
            DataTable resultDT = mdu.LoadEntries(tableName: TABLE_NAME, orderBy: "Phase", perBatch: perBatch, currentBatch: currentBatch, criteria: criteriaX);

            
            return resultDT;
        }

        public DataTable LoadFromDbViaFileNameFromOutbound(string fileName, int perBatch = 50, int currentBatch = 1)
        {
            Dictionary<string, string> criteriaX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName
            };

            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            DataTable resultDT = mdu.LoadEntries(tableName: "OutboundStrEmails", orderBy: "Phase", perBatch: perBatch, currentBatch: currentBatch, criteria: criteriaX);


            return resultDT;
        }

        /// <summary>
        /// Get Current Phase of a File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetCurrentPhaseViaFileName(string fileName)
        {
            string phaseString = "";
            DataTable dtX = LoadFromDbViaFileNameFromOutbound(fileName);
            if (dtX.Rows.Count > 0)
            {
                 phaseString = (dtX.AsEnumerable().OrderBy(drX => drX["Phase"])?.LastOrDefault())["Phase"]?.ToString() ?? string.Empty;
            }

            return phaseString;
        }


        /// <summary>
        /// Determine approval
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsApproved(string fileName, int cutOff = 3, string inv = "")
        {
            string phaseX = GetCurrentPhaseViaFileName(fileName);
            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            Dictionary<string, string> countCritX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName,
                ["Phase"] = phaseX,
                ["Response"] = "APPROVED"
            };

            if (!string.IsNullOrEmpty(inv))
            {
                countCritX["Inv"] = inv.Trim();
            }

            long resultCt = mdu.CountEntries(TABLE_NAME, countCritX);
            Console.WriteLine($"Approval Count >>> {resultCt} | {phaseX}");

            return resultCt >= cutOff;
        }


        /// <summary>
        /// Determine approval based on majority
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public bool IsDisapproved(string fileName)
        //{
        //    string phaseX = GetCurrentPhaseViaFileName(fileName);

        //    StrEmailsRecipientsHandler strRecipientsH = new StrEmailsRecipientsHandler(this.DbPath);
        //    DataTable IsAvailableDt = strRecipientsH.LoadFromDbIsAvailable(phaseX);
        //    Double IsAvailableCount = IsAvailableDt.AsEnumerable().Count();
        //    Double cutOff = IsAvailableCount / 2.0;

        //    // Disapproval count
        //    MyDbUtils mdu = new MyDbUtils(this.DbPath);
        //    Dictionary<string, string> countCritX = new Dictionary<string, string>()
        //    {
        //        ["FileName"] = fileName,
        //        ["Phase"] = phaseX,
        //        ["Response"] = "REJECTED"
        //    };
        //    Double resultCt = mdu.CountEntries(TABLE_NAME, countCritX);

        //    // Overall count of responses
        //    MyDbUtils mduY = new MyDbUtils(this.DbPath);
        //    Dictionary<string, string> countCritY = new Dictionary<string, string>()
        //    {
        //        ["FileName"] = fileName,
        //        ["Phase"] = phaseX,
        //    };
        //    Double overallCt = mduY.CountEntries(TABLE_NAME, countCritY);


        //    if (overallCt < cutOff) // not enuf respondents
        //    {
        //        return false; // dont label as disapproved
        //    }

        //    if (resultCt == 0) // non existing filename
        //    {
        //        return false;
        //    }
        //    return resultCt >= cutOff;  // True if REJECTED count is more than the cutOff count
        //}



        /// <summary>
        /// Determine approval if 3 or more REJECTED
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsDisapproved(string fileName, int cutOff = 3)
        {
            
            string phaseX = GetCurrentPhaseViaFileName(fileName);

            // Disapproval count
            MyDbUtils mdu = new MyDbUtils(this.DbPath);
            Dictionary<string, string> countCritX = new Dictionary<string, string>()
            {
                ["FileName"] = fileName,
                ["Phase"] = phaseX,
                ["Response"] = "REJECTED"
            };
            Double resultCt = mdu.CountEntries(TABLE_NAME, countCritX);

            
            if (resultCt >= cutOff) // True if there are 3 or REJECTED
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
    }
}
