using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Add entry to InboundStrEmails Table
    /// </summary>
    public sealed class InsertOutboundStrEmailsToDb : NativeActivity<int>
    {
        #region .    Activity Name    .
        public InsertOutboundStrEmailsToDb()
        {
            this.DisplayName = "Insert Inbound Str Emails To Db";
        }
        #endregion

        #region .    UiPath Properties    .


        [RequiredArgument, Category("Input"), Description("Db Path")]
        public InArgument<string> In_DbPath
        {
            get;
            set;
        }



        [RequiredArgument, Category("Input"), Description("FileName")]
        public InArgument<string> In_FileName
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Email Subject")]
        public InArgument<string> In_Subject
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Comma separated List of Email Username")]
        public InArgument<string> In_EmailUsernames
        {
            get;
            set;
        }


        [RequiredArgument, Category("Input"), Description("Sent date")]
        public InArgument<string> In_SentDate
        {
            get;
            set;
        }



        #endregion

        #region .    Methods    .

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string dbPath = this.In_DbPath.Get(context);
                string fileName = this.In_FileName.Get(context);
                string subject = this.In_Subject.Get(context);
                string emailUsernames = this.In_EmailUsernames.Get(context);
                string sentDate = this.In_SentDate.Get(context);
                

                OutboundStrEmailsHandler iSEHandler = new OutboundStrEmailsHandler(dbPath, fileName, subject, emailUsernames, sentDate);
                iSEHandler.InsertToDb();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[InsertOutboundStrEmailsToDb] Exception  >>> {e.Message}");
                throw e;
            }
        }

        #endregion
    }

}
