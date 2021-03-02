using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace StrEmailMonitoring
{
    /// <summary>
    /// Add entry to InboundStrEmails Table
    /// </summary>
    public sealed class InsertInboundStrEmailsToDb : NativeActivity<int>
    {
        #region .    Activity Name    .
        public InsertInboundStrEmailsToDb()
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

        [RequiredArgument, Category("Input"), Description("Email address")]
        public InArgument<string> In_EmailUsername
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Response")]
        public InArgument<string> In_Response
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Response date")]
        public InArgument<string> In_ResponseDate
        {
            get;
            set;
        }

        [Category("Input"), Description("Investigator initials")]
        public InArgument<string> In_Inv
        {
            get;
            set;
        }

        [RequiredArgument, Category("Input"), Description("Email Body")]
        public InArgument<string> In_Message
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
                string emailUsername = this.In_EmailUsername.Get(context);
                string response = this.In_Response.Get(context);
                string responseDate = this.In_ResponseDate.Get(context);
                string inv = this.In_Inv.Get(context);
                string msg = this.In_Message.Get(context);

                InboundStrEmailsHandler iSEHandler = new InboundStrEmailsHandler(dbPath, fileName, subject, emailUsername, response, responseDate, inv, msg);
                iSEHandler.InsertToDb();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[InsertInboundStrEmailsToDb] Exception  >>> {e.Message}");
                throw e;
            }
        }

        #endregion
    }

}
